using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float _movementSpeed=1;
    public float _movementTime=5;
    public float _rotationAmmount=5f;
    public Vector3 _zoomAmmount=new Vector3(0,-10,10);
    [SerializeField]
    private float _minZoom, _maxZoom=0;
    [SerializeField]
    private Camera _camera;


    public Vector3 _newPosition;
    public Quaternion _newRotation;
    public Vector3 _newZoom;
    // Start is called before the first frame update
    void Start()
    {
        _camera = _camera ?? Camera.main;
        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = _camera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {

        _newZoom += _zoomAmmount * Input.GetAxis("Mouse ScrollWheel");
        _newPosition += (transform.forward * _movementSpeed * Input.GetAxis("Vertical")*_newZoom.y);
        _newPosition += (transform.right * _movementSpeed * Input.GetAxis("Horizontal")*_newZoom.y);
        _newRotation *= Quaternion.Euler(Vector3.up * _rotationAmmount * -Input.GetAxis("Rotation"));


        _newZoom.y = Mathf.Clamp(_newZoom.y, _minZoom, _maxZoom);
        _newZoom.z = Mathf.Clamp(_newZoom.z, -_maxZoom, -_minZoom);

        transform.position = Vector3.Lerp(transform.position, _newPosition, _movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _movementTime);
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _newZoom, _movementTime * Time.deltaTime);

    }
}
