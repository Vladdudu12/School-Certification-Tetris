using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GhostPiece : MonoBehaviour
{

    private bool _isHeld;
    [SerializeField]
    private int _id;

    //make piece transparent

    private void Start()
    {
        GetTransparent();
    }

    private void Update()
    {
        try
        {
            GetGhostPos();

        }
        catch(IndexOutOfRangeException e)
        {
            throw new ArgumentException("index is out of range", e);
        }
        //MoveDown();
    }

    public void GetTransparent()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        }
    }

    private void MoveDown()
    {
        while (isValidPos())
        {
            transform.position += Vector3.down;
        }
        if (!isValidPos())
        {
            transform.position += Vector3.up;
        }
    }

    private void GetGhostPos()
    {
        Transform piecePos = GameObject.FindGameObjectWithTag("activePiece").transform;
        if (piecePos != null)
        {
            if (Math.Abs(piecePos.transform.position.y) - Math.Abs(this.transform.position.y) >= -1) 
            {
                if (piecePos.transform.position.y >= 4f)
                {
                    this.transform.position = new Vector3(piecePos.position.x, piecePos.position.y - 4f, piecePos.position.z);
                }
                //else if (piecePos.transform.position.y == 3f)
                //{
                //    this.transform.position = new Vector3(piecePos.position.x, piecePos.position.y - 3f, piecePos.position.z);
                //}
                //else if (piecePos.transform.position.y == 2f)
                //{
                //    this.gameObject.SetActive(false);
                //                    //this.transform.position = new Vector3(piecePos.position.x, piecePos.position.y - 2f, piecePos.position.z);
                //}
                //else if (piecePos.transform.position.y <= 1f)
                //{
                //    this.gameObject.SetActive(false);
                //    //this.transform.position = new Vector3(piecePos.position.x, piecePos.position.y - 1f, piecePos.position.z);
                //}


                //this.transform.position = new Vector3(piecePos.position.x, piecePos.position.y - 4f, piecePos.position.z);
            }
            else
            {                
                this.gameObject.SetActive(false);
            }
            MoveDown();

            this.transform.rotation = piecePos.rotation;
        }
    }

    private bool isValidPos()
    {
        if (_isHeld == false)
        {
            foreach (Transform child in transform)
            {
                Vector2 v = Playfield.roundVector2(child.position);

                if (!Playfield.insideBorder(v))
                    return false;

                //Debug.Log("v.x: " + v.x.ToString()) ;
                //Debug.Log("v.y " + v.y.ToString());
                if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                    Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                    return false;

            }
            return true;
        }
        else
        {
            return true;
        }

    }


    public int getId()
    {
        return _id;
    }
}
