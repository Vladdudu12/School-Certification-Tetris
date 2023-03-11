using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _lastFall = 0;
    [SerializeField]
    private float[] _fallMultiplier;
    [SerializeField]
    private int _multiplierIndex;
    private float _moveTimer;
    [SerializeField]
    private int _moveCounter;
    [SerializeField]
    private bool _isHeld;
    [SerializeField]
    private bool _canDrop = true;
    [SerializeField]
    private bool _canMove = true;
    [SerializeField]
    private int _id;

    [SerializeField]
    private GameObject _assignedGhost;

    private GameManager _gameManager;

    [SerializeField]
    private AudioSource _soundFxSource;
    [SerializeField]
    private AudioClip[] _lineSoundClips;
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }

        _soundFxSource = GameObject.Find("SoundFx").GetComponent<AudioSource>();
        if (_soundFxSource == null)
        {
            Debug.LogError("The sound fx source is NULL");
        }

        _moveTimer = Time.timeSinceLevelLoad + 0.5f;
        Debug.Log("Move timer" + _moveTimer);
        if(!isValidPos())
        {
            _gameManager.GameOver();
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }


        this.tag = "activePiece";
        _multiplierIndex = _gameManager._level;
        Debug.Log("Multiplier Index: " + _multiplierIndex);
        Debug.Log("Falling Multiplier: " + _fallMultiplier[_multiplierIndex]);
    }

    private void Update()
    {
        if (!CheckCanDrop())
        {
            _canDrop = false;
        }
        if (_gameManager._isPause == false)
        {
            if (_moveCounter > 2)
            {
                _canDrop = false;
            }

            //left
            if (Input.GetKey(KeyCode.LeftArrow) && Time.timeSinceLevelLoad - _moveTimer >= .1 && _canMove == true)
            {
                transform.position += new Vector3(-1, 0, 0);

                if (isValidPos())
                {
                    updateGrid();
                    _moveTimer = Time.timeSinceLevelLoad;
                }
                else
                {
                    transform.position += new Vector3(1, 0, 0);
                    DownMovement();
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) && Time.timeSinceLevelLoad - _moveTimer >= .1 && _canMove == true) //right
            {
                transform.position += new Vector3(1, 0, 0);

                if (isValidPos())
                {
                    updateGrid();
                    _moveTimer = Time.timeSinceLevelLoad;
                }
                else
                {
                    transform.position += new Vector3(-1, 0, 0);
                    DownMovement();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && _canMove == true) //rotate
            {
                transform.Rotate(0, 0, -90);

                if (isValidPos())
                {
                    updateGrid();
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.timeSinceLevelLoad - _moveTimer >= .1 || Time.timeSinceLevelLoad - _lastFall >= 1 / _fallMultiplier[_multiplierIndex] && _canMove == true) //fall
            {
                transform.position += new Vector3(0, -1, 0);

                if (isValidPos())
                {
                    updateGrid();
                    _moveTimer = Time.timeSinceLevelLoad;
                    _moveCounter++;
                }
                else
                {
                    transform.position += new Vector3(0, 1, 0);
                    _moveCounter--;
                    int deletedRows = Playfield.deleteFullRows();
                    if (deletedRows == 4)
                    {
                        _gameManager.AddScore(800);
                        StartCoroutine(ChangeClipRoutine(deletedRows));

                    }
                    else if (deletedRows > 0)
                    {
                        _gameManager.AddScore(100 * deletedRows);
                        StartCoroutine(ChangeClipRoutine(deletedRows));

                    }

                    FindObjectOfType<Spawner>().ChangeGameObjectTag(this.gameObject);
                    FindObjectOfType<Spawner>().FirstElement();
                    FindObjectOfType<Spawner>().LetHold();

                    enabled = false;

                }

                _lastFall = Time.timeSinceLevelLoad;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _canDrop == true)
            {
                _canDrop = false;
                _canMove = false;
                this.transform.position = _assignedGhost.transform.position;
                StartCoroutine(ChangePieceRoutine());
                updateGrid();

                int deletedRows = Playfield.deleteFullRows();
                if (deletedRows == 4)
                {
                    _gameManager.AddScore(800);
                    StartCoroutine(ChangeClipRoutine(deletedRows));

                }
                else if (deletedRows > 0)
                {
                    _gameManager.AddScore(100 * deletedRows);
                    StartCoroutine(ChangeClipRoutine(deletedRows));

                }

                enabled = false;
            }

        }


    }

    private IEnumerator ChangePieceRoutine()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.onGhostDisable();
        spawner.ChangeGameObjectTag(this.gameObject);
        //Debug.Break();
        yield return new WaitForSeconds(0.5f);
        spawner.FirstElement();
        spawner.LetHold();
        //Debug.Break();
    }

    private void DownMovement()
    {
        Debug.Log("difference" + (Time.timeSinceLevelLoad - _moveTimer).ToString());
        if (Input.GetKey(KeyCode.DownArrow) && Time.timeSinceLevelLoad - _moveTimer >= 0.1f || Time.timeSinceLevelLoad - _lastFall >= 1/_fallMultiplier[_multiplierIndex]) //fall
        {
            transform.position += new Vector3(0, -1, 0);

            if (isValidPos())
            {
                updateGrid();
                _moveTimer = Time.timeSinceLevelLoad;
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);

                int deletedRows = Playfield.deleteFullRows();
                if (deletedRows == 4)
                {
                    _gameManager.AddScore(800);
                    StartCoroutine(ChangeClipRoutine(deletedRows));
                }
                else if (deletedRows > 0)
                {
                    _gameManager.AddScore(100 * deletedRows);
                    StartCoroutine(ChangeClipRoutine(deletedRows));
                }
                FindObjectOfType<Spawner>().ChangeGameObjectTag(this.gameObject);
                FindObjectOfType<Spawner>().FirstElement();
                FindObjectOfType<Spawner>().LetHold();

                //Debug.Log(this.gameObject.tag);
                //this.gameObject.tag = "inactivePiece";
                //Debug.Log(this.gameObject.tag);
                enabled = false;

            }

            _lastFall = Time.timeSinceLevelLoad;
        }
    }

    IEnumerator ChangeClipRoutine(int index)
    {
        AudioSource soundFxSource = GameObject.Find("SoundFx").GetComponent<AudioSource>();

        yield return new WaitForSeconds(soundFxSource.clip.length);
        soundFxSource.clip = _lineSoundClips[index - 1];
        soundFxSource.Play();
    }

    private bool CheckCanDrop()
    {
        for (int i = 0; i < Playfield.width; i++)
        {
            if (Playfield.grid[i,8] != null)
            {
                Debug.LogError(Playfield.grid[i, 8].transform.name);

                return false;
            }
        }
        return true;
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
                {
                    //Debug.Log(this.gameObject.tag);
                    this.gameObject.tag = "inactivePiece";
                    //Debug.Log(this.gameObject.tag);
                    return false;
                }
            }
            return true;
        }
        else
        {
            return true;
        }

    }

    //private void HardDrop()
    //{
    //    for (int i = Playfield.width; i > 0; i++)
    //    {
    //        for (int j = Playfield.height; j > 0; j++) 
    //        {
    //            if (Playfield.grid[i, j] != null)
    //            {

    //            }
    //        }
    //    }

    //    int j = Playfield.height;
    //    int i = Playfield.width;
    //    while (j > 0)
    //    {
    //        if (Playfield.grid[i, j] != null && Playfield.grid[i, j].parent != transform)
    //        {
    //            i++;
    //            j = Playfield.height; 
    //        }
    //        else
    //        {

    //        }
    //    }
    //}

    private void updateGrid()
    {
        for (int j = 0; j < Playfield.height; j++)
        {
            for (int i = 0; i < Playfield.width; i++)
            {
                if (Playfield.grid[i, j] != null)
                {
                    if (Playfield.grid[i, j].parent == transform)
                    {
                        Playfield.grid[i, j] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVector2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public void deleteGridPos()
    {
        for (int j = 0; j < Playfield.height; j++) 
        {
            for (int i = 0; i < Playfield.width; i++)
            {
                if (Playfield.grid[i, j] != null)
                {
                    if (Playfield.grid[i, j].parent == transform)
                    {
                        Playfield.grid[i, j] = null;
                    }
                }
            }
        }
    }

    public void Held()
    {
        _isHeld = !_isHeld;
    }


    public int getId()
    {
        return _id;
    }

    public void assignGhost(GameObject ghostPiece)
    {
        _assignedGhost = ghostPiece;
    }
}
