using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pieces;

    [SerializeField]
    private GameObject[] ghostPieces;
    /// <summary>
    /// List of next 3 pieces
    /// </summary>

    [SerializeField]
    private GameObject[] _nextPieces = new GameObject[5];
    [SerializeField]
    private GameObject[] _nextGhostPieces = new GameObject[5];
    [SerializeField]
    private Vector3[] _nextPiecesPos = new Vector3[5];

    [SerializeField]
    private bool _canHold = true;
    [SerializeField]
    private GameObject _holdPiece;
    [SerializeField]
    private GameObject _holdGhostPiece;
    [SerializeField]
    private GameObject _lastPiece;
    [SerializeField]
    private GameObject _lastGhostPiece;
    [SerializeField]
    private Vector3[] _holdPos;
    [SerializeField]
    private Vector3 _startPos;

    private GameManager _gameManager;
    private Playfield _playfield;
    private void Start()
    {
        _startPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        _canHold = true;
        _holdPiece = null;

        _playfield = GameObject.Find("Borders").GetComponent<Playfield>();
        if (_playfield == null)
        {
            Debug.LogError("the playfield is NULL");
        }
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL");
        }
        StartCoroutine(PopulateListRoutine());
        FirstElement();
    }

    private void Update()
    {
        Hold();
    }

    private IEnumerator PopulateListRoutine()
    {
        for (int i = 0; i < _nextPieces.Length; i++)
        {
            SpawnNext(i, _nextPieces, _nextGhostPieces);
        }
        yield return null;
    }
    public void SpawnNext(int i, GameObject[] pieceList, GameObject[] ghostPieceList)
    {
        if (_gameManager._isGameOver == false)
        {
            int index = Random.Range(0, pieces.Length);
            pieceList[i] = Instantiate(pieces[index], _startPos, Quaternion.identity);
            pieceList[i].GetComponent<PieceBehaviour>().enabled = false;
            pieceList[i].GetComponent<PieceBehaviour>().deleteGridPos();
            pieceList[i].transform.parent = GameObject.FindGameObjectWithTag("PieceList").transform;


            pieceList[i].transform.tag = "inactivePiece";

            int id = pieceList[i].GetComponent<PieceBehaviour>().getId();
            ChangeListPosition(id, i, pieceList);

            ghostPieceList[i] = Instantiate(ghostPieces[index], _startPos, Quaternion.identity);
            pieceList[i].GetComponent<PieceBehaviour>().assignGhost(ghostPieceList[i]);
            ghostPieceList[i].GetComponent<GhostPiece>().GetTransparent();
            ghostPieceList[i].GetComponent<GhostPiece>().enabled = false;
            ghostPieceList[i].transform.parent = GameObject.FindGameObjectWithTag("GhostPieceList").transform;
            ChangeListPosition(id, i, ghostPieceList);
            ghostPieceList[i].SetActive(false);
        }
    }


    /// <summary>
    /// spawn 3 pieces and 3 ghost pieces
    /// spawn first piece and ghost piece
    /// ghost piece over first piece
    /// </summary>

    public void FirstElement()
    {
        //populate list
        //get first from the list
        //move list upwards
        //populate last point

        if (_gameManager._isGameOver == false)
        {
            if (_lastPiece == null)
            {
                _lastPiece = _nextPieces[0];
                _lastPiece.transform.tag = "activePiece";
                _lastGhostPiece = _nextGhostPieces[0];
                _lastGhostPiece.SetActive(true);
                _lastPiece.GetComponent<PieceBehaviour>().enabled = true;
                _lastGhostPiece.GetComponent<GhostPiece>().enabled = true;
                _lastPiece.transform.position = _startPos;
                _lastGhostPiece.transform.position = new Vector3(_startPos.x, _startPos.y - 2f, _startPos.z);
                //_nextPieces[0] = _nextPieces[1];
                //_nextPieces[0].transform.position = _nextPiecesPos[0];
                //_nextPieces[1] = _nextPieces[2];
                //_nextPieces[1].transform.position = _nextPiecesPos[1];
                //_nextPieces[2] = null;
                MoveUpList();
                SpawnNext(4, _nextPieces, _nextGhostPieces);
            }
            else
            {
                _lastGhostPiece.SetActive(false);
                _lastPiece = _nextPieces[0];
                _lastPiece.transform.tag = "activePiece";
                _lastGhostPiece = _nextGhostPieces[0];
                _lastGhostPiece.SetActive(true);
                _lastPiece.GetComponent<PieceBehaviour>().enabled = true;
                _lastGhostPiece.GetComponent<GhostPiece>().enabled = true;
                _lastPiece.transform.position = _startPos;
                _lastGhostPiece.transform.position = new Vector3(_startPos.x, _startPos.y - 2f, _startPos.z);
                //_nextPieces[0] = _nextPieces[1];
                //_nextPieces[0].transform.position = _nextPiecesPos[0];
                //_nextPieces[1] = _nextPieces[2];
                //_nextPieces[1].transform.position = _nextPiecesPos[1];
                //_nextPieces[2] = null;
                MoveUpList();
                SpawnNext(4, _nextPieces, _nextGhostPieces);
            }
        }
    }

    private void MoveUpList()
    {
        for (int i = 0; i < _nextPieces.Length - 1; i++)
        {
            //Debug.Log(i);
            _nextPieces[i] = _nextPieces[i + 1];
            int id = _nextPieces[i].GetComponent<PieceBehaviour>().getId();
            ChangeListPosition(id, i, _nextPieces);

            _nextGhostPieces[i] = _nextGhostPieces[i + 1];
            id = _nextGhostPieces[i].GetComponent<GhostPiece>().getId();
            ChangeListPosition(id, i, _nextGhostPieces);
            //_nextPieces[i].transform.position = _nextPiecesPos[i];
        }
        //Debug.Log(_nextPieces.Length-1);
        _nextPieces[_nextPieces.Length - 1] = null;
        _nextGhostPieces[_nextGhostPieces.Length - 1] = null;
      
    }

    private void Hold()
    {
        if (Input.GetKeyDown(KeyCode.C) && _canHold == true)
        {
            if(_holdPiece == null)
            {
                _holdPiece = _lastPiece;
                _holdPiece.transform.tag = "holdPiece";
                _holdPiece.GetComponent<PieceBehaviour>().Held();
                _holdPiece.GetComponent<PieceBehaviour>().enabled = false;
                _holdPiece.GetComponent<PieceBehaviour>().deleteGridPos();

                _holdGhostPiece = _lastGhostPiece;
                //_holdGhostPiece.GetComponent<GhostPiece>().enabled = false;
                onGhostHold();
                //_holdPiece.transform.position = _holdPos;
                int id = _holdPiece.GetComponent<PieceBehaviour>().getId();

                ChangeHoldPosition(id);


                FirstElement();
            }
            else
            {
                var aux = _holdPiece;
                _holdPiece = _lastPiece;
                _holdPiece.transform.tag = "holdPiece";
                _lastPiece = aux;
                _holdPiece.GetComponent<PieceBehaviour>().Held();
                _holdPiece.GetComponent<PieceBehaviour>().enabled = false;
                _holdPiece.GetComponent<PieceBehaviour>().deleteGridPos();

                var aux2 = _holdGhostPiece;
                _holdGhostPiece = _lastGhostPiece;
                _lastGhostPiece = aux2;
                onGhostHold();
                //_holdGhostPiece.GetComponent<GhostPiece>().enabled = false;

                //_holdPiece.transform.position = _holdPos;
                int id = _holdPiece.GetComponent<PieceBehaviour>().getId();
                ChangeHoldPosition(id);

                _lastPiece.transform.tag = "activePiece";
                _lastPiece.GetComponent<PieceBehaviour>().Held();
                _lastPiece.GetComponent<PieceBehaviour>().enabled = true;
                // _lastGhostPiece.GetComponent<GhostPiece>().enabled = true;
                onGhostRestart();
                _lastPiece.transform.position = _startPos;
            }

            _canHold = false;

        }
    }

    public void onGhostDisable()
    {
        _lastGhostPiece.SetActive(false);
    }

    private void onGhostHold()
    {
        _holdGhostPiece.SetActive(false);
    }
    
    private void onGhostRestart()
    {
        _lastGhostPiece.SetActive(true);
    }

    private void ChangeHoldPosition(int id)
    {
        switch (id)
        {
            case 1:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 2:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[1], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 3:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[2], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 4:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[3], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 5:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[4], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 6:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[5], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
            case 7:
                _holdPiece.transform.SetPositionAndRotation(_holdPos[6], Quaternion.identity);
                _holdGhostPiece.transform.SetPositionAndRotation(_holdPos[0], Quaternion.identity);
                break;
        }
    }

    private void ChangeListPosition(int id, int index, GameObject[] pieces)
    {
        Vector3 piecePosition = new Vector3(_nextPiecesPos[index].x - 0.5f, _nextPiecesPos[index].y, _nextPiecesPos[index].z);
        //Debug.Log("The next piece is: " + _nextPieces[index].name + "piece Position: " + piecePosition);
        switch (id)
        {
            case 1:
                pieces[index].transform.SetPositionAndRotation(piecePosition, Quaternion.identity);
                break;
            case 2:
                pieces[index].transform.SetPositionAndRotation(_nextPiecesPos[index], Quaternion.identity);
                break;
            case 3:
                pieces[index].transform.SetPositionAndRotation(_nextPiecesPos[index], Quaternion.identity);
                break;
            case 4:
                pieces[index].transform.SetPositionAndRotation(piecePosition, Quaternion.identity);
                break;
            case 5:
                pieces[index].transform.SetPositionAndRotation(_nextPiecesPos[index], Quaternion.identity);
                break;
            case 6:
                pieces[index].transform.SetPositionAndRotation(_nextPiecesPos[index], Quaternion.identity);
                break;
            case 7:
                pieces[index].transform.SetPositionAndRotation(_nextPiecesPos[index], Quaternion.identity);
                break;
        }
    }


    public void LetHold()
    {
        _canHold = true;
    }

    public void ChangeGameObjectTag(GameObject gameObject)
    {
        gameObject.tag = "inactivePiece";
    }
}
