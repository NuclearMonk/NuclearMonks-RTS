using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Selected_Dictionary))]
public class SelectionController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Selected_Dictionary _selected_Dictionary;
    [SerializeField]
    private RectTransform _uiSelectionBox;
    private Image _uiSelectionImage;
    [SerializeField]
    private LayerMask _groundPlaneLayerMask;                  //using a plane under the actual ground if the ground is not flat is seriously advisable as to avoid gameobjects going unselected because they get under the inclined mesh;
    [SerializeField]
    private LayerMask _clickableLayer;


    private bool _isDragSelect;
    private Vector3 _uiClickStart;
    private Vector3 _uiClickEnd;
    private float _width, _height;
    private Vector3 p1, p2;
    [SerializeField]
    MeshCollider _meshCollider;
    private WaitForSeconds WaitForABit= new WaitForSeconds(0.2f);

    private void Awake()
    {
        //gets the image so it can be enabled and disabled instead of destroyed for perforamnce reasons.
        _uiSelectionImage = _uiSelectionBox.GetComponent<Image>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _uiClickStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _uiClickEnd = Input.mousePosition;
            UpdateSelectionBox();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_isDragSelect)                         
            {                                          
                if (!Input.GetKey(KeyCode.LeftShift)) _selected_Dictionary.RemoveSelections();
                CastSelectionArea();
                DisableSelectionBox();
            }
            else SingleClick(_uiClickStart);
        }
    }
    void UpdateSelectionBox()
    {
        p1 = _uiClickStart;
        p2 = _uiClickEnd;
        if (Vector3.SqrMagnitude(p1 - p2) > 20f)
        {
            EnableSelectionBox();                                                 
            Vector3 swap = p1;                      //this function makes sure that p1 and p2 are always in the same relative position on our box.                    
            if (p1.x > p2.x)                        //  -----P2
            {                                       //  |     |
                p1.x = p2.x;                        //  |     |
                p2.x = swap.x;                      //  P1-----
            }
            if (p1.y > p2.y)
            {
                p1.y = p2.y;
                p2.y = swap.y;
            }
            _width = p2.x - p1.x;
            _height = p2.y - p1.y;
            if (_width == 0)
            {
                _width = 1;
            }
            if (_height == 0)
            {
                _height = 1;
            }
            _uiSelectionBox.position = p1;
            _uiSelectionBox.sizeDelta = (new Vector2(_width, _height));
        }
    }

    void EnableSelectionBox()
    {
        _isDragSelect = true;
        _uiSelectionImage.enabled = true;
    }

    void DisableSelectionBox()
    {
        _isDragSelect = false;
        _uiSelectionImage.enabled = false;
    }

    private void CastSelectionArea()
    {
        Vector3[] points= new Vector3[5];

        Ray[] rays ={                                                               //will always be cast in this order
            _camera.ScreenPointToRay(p1),                                           //1---------2
            _camera.ScreenPointToRay(new Vector3(p1.x,p1.y+_height,p1.z)),          //|         |
            _camera.ScreenPointToRay(p2),                                           //|         |
            _camera.ScreenPointToRay(new Vector3(p2.x,p2.y-_height,p2.z)),          //0---------3
        };
        int i = 0;
        foreach (Ray ray in rays)
        {
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000f,_groundPlaneLayerMask);
            points.SetValue(hit.point, i);
            i++;
        }
        points.SetValue(points[3] + Vector3.up * 0.01f, 3);                       //this avoids the case of the ground being perfectly flat and generating a perfectly planar quad
        points.SetValue(_camera.transform.position,4);
        _meshCollider.sharedMesh= GenerateMesh(points);
        _meshCollider.convex = true;
        _meshCollider.enabled = true;
        StartCoroutine(DisableMeshcoliderCoroutine());


    }

    private Mesh GenerateMesh(Vector3[] vertices)
    {
        int[] trigOrder ={
            2,1,0,
            0,3,2,
            0,1,4,
            1,2,4,
            2,3,4,
            3,0,4

        };
        Mesh mesh= new Mesh();
        mesh.RecalculateNormals();
        mesh.vertices = vertices;
        mesh.triangles = trigOrder;
        return mesh;
    }
    private void OnTriggerEnter(Collider other)
    {
            _selected_Dictionary.AddSelection(other.gameObject);
        
    }
    private void SingleClick(Vector3 uiClickPosition)
    {
        Debug.Log("singleClick", this);
        RaycastHit hit;
        Physics.Raycast(_camera.ScreenPointToRay(_uiClickStart), out hit, 1000f,_clickableLayer);

        if (hit.collider == null) return;
        Debug.Log(hit.collider.name, this);
        ISelectable select =_selected_Dictionary.GameObjectSelectable(hit.collider.gameObject);
        if (select!=null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _selected_Dictionary.ToggleSelection(hit.collider.gameObject);

            }
            else
            {
                _selected_Dictionary.RemoveSelections();
                _selected_Dictionary.AddSelection(hit.collider.gameObject);
            }

        }
        else
        {
            foreach(UnitController unit in _selected_Dictionary.SelectedUnitControlers())
            {
                unit.UpdateDestination(hit.point);
            }
        }
    }
    private IEnumerator DisableMeshcoliderCoroutine()
    {
        yield return WaitForABit;
        _meshCollider.enabled = false;
    }
}
