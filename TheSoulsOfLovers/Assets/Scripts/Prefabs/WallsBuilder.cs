using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsBuilder : MonoBehaviour
{
    public GameObject wall; //объект одной стенки
    public GameObject wall_up; // верхняя стенка
    private void Start()
    {
        LineWall();
    }

    void LineWall()
    {
        
       float differentAxisY = 3.75f;
       //LEFT and RiGHT ROOM
       float LRfirst = 11f;
       float RRfirst = 11.2f;
       for (int i = 0; i < 3; i++ ) {
            Vector3 wallPosition = new Vector3(-15.1f, LRfirst-differentAxisY);
            Vector3 wallPosition2 = new Vector3(14.7f, RRfirst - differentAxisY);          
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall, wallPosition, wallRotation);
            Instantiate(wall, wallPosition2, wallRotation);
            LRfirst = LRfirst - differentAxisY;
            RRfirst = RRfirst - differentAxisY;
        }
      
        //UPPER ROOM
        float URfirst = 18.4f;
        for (int k = 0; k < 3; k++)
        {
            Vector3 wallPosition = new Vector3(-4.6f, URfirst - differentAxisY);
            Vector3 wallPosition2 = new Vector3(4.75f, URfirst - differentAxisY);
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall, wallPosition, wallRotation);
            Instantiate(wall, wallPosition2, wallRotation);
            URfirst = URfirst - differentAxisY;
        }

        //CENTER ROOM
        float CRfirst = -0.73f;
        for (int k = 0; k < 2; k++)
        {
            Vector3 wallPosition = new Vector3(-4.54f, CRfirst - differentAxisY);
            Vector3 wallPosition2 = new Vector3(4.8f, CRfirst - differentAxisY);
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall, wallPosition, wallRotation);
            Instantiate(wall, wallPosition2, wallRotation);
            CRfirst = CRfirst - differentAxisY;
        }

        //LOWER ROOM
        float LLRfirst = -11.1f;
        for (int k = 0; k < 2; k++)
        {
            Vector3 wallPosition = new Vector3(-4.54f, LLRfirst - differentAxisY);
            Vector3 wallPosition2 = new Vector3(4.8f, LLRfirst - differentAxisY);
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall, wallPosition, wallRotation);
            Instantiate(wall, wallPosition2, wallRotation);
            LLRfirst = LLRfirst - differentAxisY;
        }



        //LOWER ROOM (WALL UP)
        float LRWUfirst = -2.3f;
        for (int k = 0; k < 2; k++)
        {
            Vector3 wallPosition = new Vector3(LRWUfirst, -12.5f);         
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall_up, wallPosition, wallRotation);   
            LRWUfirst = LRWUfirst + 4.8f;
        }

        //LEFT AND RIGHT ROOM (WALL UP)
        float LRWUfirst1 = -12.80f;
        float LRWUfirst2 = 7.74f;
        for (int k = 0; k < 2; k++)
        {
            Vector3 wallPosition = new Vector3(LRWUfirst1, -2.22f);
            Vector3 wallPosition2 = new Vector3(LRWUfirst2, -2.22f);
            Quaternion wallRotation = new Quaternion();
            Instantiate(wall_up, wallPosition, wallRotation);
            Instantiate(wall_up, wallPosition2, wallRotation);
            LRWUfirst1 = LRWUfirst1 + 5.20f;
            LRWUfirst2 = LRWUfirst2 + 4.65f;
        }

        //!!!ДОБАВИТЬ БОКОВЫЕ 4 СТЕНКИ



    }
}
