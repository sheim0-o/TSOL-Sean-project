using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueTypeVisitor
{
    [SerializeField]
    private TextMeshProUGUI m_SpeakerText;
    [SerializeField]
    private TextMeshProUGUI m_DialogueText;

    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private bool m_ListenToInput = false;
    private DialogueType m_NextType = null;

    private List<Button> lastButtons;

    private void Awake()
    {
        lastButtons = new List<Button>();
        m_DialogueChannel.OnDialogueTypeStart += OnDialogueTypeStart;
        m_DialogueChannel.OnDialogueTypeEnd += OnDialogueTypeEnd;

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueTypeEnd -= OnDialogueTypeEnd;
        m_DialogueChannel.OnDialogueTypeStart -= OnDialogueTypeStart;
    }

    

    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null && Input.GetAxis("Vertical") != 0 && lastButtons.Count!=0)
            lastButtons.Last().Select();

        if (m_ListenToInput && Input.GetButtonDown("Submit"))
        {
            m_DialogueChannel.RaiseRequestDialogueType(m_NextType);
        }
    }

    private void OnDialogueTypeStart(DialogueType type)
    {
        gameObject.SetActive(true);

        m_DialogueText.text = type.DialogueLine.Text;
        m_SpeakerText.text = type.DialogueLine.Speaker.CharacterName;

        m_ChoicesBoxTransform.transform.GetChild(0).gameObject.SetActive(true);

        type.Accept(this);
    }

    private void OnDialogueTypeEnd(DialogueType type)
    {
        m_NextType = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            if (m_ChoicesBoxTransform.transform.GetChild(0) != child)
                Destroy(child.gameObject);
        }
        lastButtons = new List<Button>();

        //Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    public void Visit(OneLineDialogueType type)
    {
        m_ListenToInput = true;
        m_NextType = type.NextType;
    }

    public void Visit(ChoiceDialogueType type)
    {
        m_ChoicesBoxTransform.gameObject.SetActive(true);

        var positionBtn = m_ChoicesBoxTransform.transform.GetChild(0).gameObject.transform.position;
        var x = positionBtn.x;
        var y = positionBtn.y;

        

        for (int i = 0; i < type.Choices.Length; i++)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            newChoice.transform.position = new Vector2(x, y+(1*i));
            newChoice.GetComponent<Button>().image.enabled = true;
            newChoice.GetComponent<Button>().GetComponent<Button>().enabled = true;
            newChoice.GetComponent<Button>().GetComponent<UIDialogueChoiceController>().enabled = true;
            //newChoice.GetComponent<TextMeshPro>().enabled = true;
            //newChoice.GetComponent<TextMeshPro>().GetComponent<TMP_Text>().enabled = true;
            //Transform child = transform.GetChild(0);
            //Debug.Log(child.name);
            //newChoice.GetComponent<Button>().GetComponent<TMP_Text>().enabled = true;
            //GameObject my_object = GameObject.Find("TextBtn");
            //my_object.GetComponent<TMP_Text>().enabled = true;
            newChoice.GetComponent<Button>().transform.GetChild(0).GetComponent<TMP_Text>().enabled = true ;
            
            newChoice.Choice = type.Choices[i];
            lastButtons.Add(newChoice.gameObject.GetComponent<Button>());
        }
        lastButtons.Last().Select();

        /*
        UIDialogueChoiceController newChoice = m_ChoiceControllerPrefab;
        newChoice.GetComponent<Button>().image.enabled = true;
        newChoice.GetComponent<Button>().GetComponent<Button>().enabled = true;
        newChoice.GetComponent<Button>().GetComponent<UIDialogueChoiceController>().enabled = true;
        newChoice.GetComponent<Button>().transform.GetChild(0).GetComponent<TMP_Text>().enabled = true;

        for (int i = 0;i< type.Choices.Length;i++)
        {
            DialogueChoice choice = type.Choices[i];
            newChoice.Choice = choice;
            UIDialogueChoiceController newButton = Instantiate(newChoice, new Vector2(x, y+(80*i)), Quaternion.identity, m_ChoicesBoxTransform);
            if (i == type.Choices.Length - 1)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(newButton.gameObject);


                //eventSystem.SetSelectedGameObject(newButton.gameObject);
                //eventSystem.firstSelectedGameObject = newButton.gameObject;
            }
        }
        */

        m_ChoicesBoxTransform.transform.GetChild(0).gameObject.SetActive(false);
    }
}
