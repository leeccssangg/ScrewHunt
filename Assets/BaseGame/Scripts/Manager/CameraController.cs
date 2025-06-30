using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.CustomType;
using TW.Utility.DesignPattern;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : Singleton<CameraController>
{
   private Transform m_MainCameraTransform;
    public Transform MainCameraTransform { get { return m_MainCameraTransform ??= MainCamera.transform; } }
    
    [field: Header("Camera Controller")]
    [field: SerializeField] private Transform CameraPivot { get; set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    //[field: SerializeField] private Camera UICamera { get; set; }
    [field: SerializeField] private Camera RaycastCamera { get; set; }
    [field: SerializeField] private float SmoothMoveTime { get; set; }
    [field: SerializeField] private float MoveIntensity { get; set; }
    [field: SerializeField] private float SmoothZoomTime { get; set; }
    [field: SerializeField] private float ZoomAmount { get; set; }
    [field: SerializeField] private float ZoomTarget { get; set; }
    // [field: SerializeField] public float MinZoom { get; set; }
    // [field: SerializeField] public float MaxZoom { get; set; }
    [field: SerializeField] private Transform CameraFlexRotation { get; set; }
    [field: SerializeField, FloatRangeEditor] public FloatRange ZoomRange {get; private set;}
    [field: SerializeField, FloatRangeEditor] private FloatRange MoveRangeX { get; set; }
    [field: SerializeField, FloatRangeEditor] private FloatRange MoveRangeY { get; set; }
    [field: SerializeField] private AnimationCurve MoveIntensityRange { get; set; }
    private bool IsZooming { get; set; }
    private FloatRange m_FixedRange;
    private Vector3 m_StartMousePosition;
    private Vector3 m_StartCameraPosition;
    private Vector3 m_CurrentMousePosition;
    private bool m_IsMouseClick;
    private bool m_IsTouchingUI;
    private Vector3 m_NewPosition;
    private Plane m_Plane;
    private float m_LastSize;
    private Vector3 m_ScreenStartMousePosition;

    private Vector3 m_Velocity1 = Vector3.zero;
    private float m_Velocity2 = 0;

    private float m_Size;
    private float m_Difference;
    private float m_StartSize;
    private float m_StartMagnitude;
    private float m_CurrentMagnitude;
    private readonly RaycastHit[] m_Results = new RaycastHit[20];
    [field: Header("Interact Controller")]
    [field: SerializeField] private LayerMask WhatIsInteractObject { get; set; }


    private void Awake()
    {
        m_Plane = new Plane(Vector3.forward, new Vector3(0, 0, 20));
        m_NewPosition = CameraPivot.position;
    }
    // private void Update()
    // {
    //     HandleMouseInput();
    //     MoveCamera();
    // }

    public void ResetLevelCamera()
    {
        CameraPivot.position = Vector3.zero;   
        m_NewPosition = CameraPivot.position;
        MainCamera.orthographicSize = 15.5f;
        RaycastCamera.orthographicSize = 15.5f;
        SetCameraSize(ZoomRange.m_Max);
    }

    private void HandleMouseInput()
    {
        if (IsPointerOverGameObject()) return;
        HandleCameraZoom();
        HandleCameraMovement();
    }
    private void HandleCameraMovement()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount != 1) return;
#endif
        if (Input.GetKeyDown(KeyCode.Mouse0) || IsZooming)
        {
            IsZooming = false;
            Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
            if (m_Plane.Raycast(ray, out float entry))
            {
                m_StartMousePosition = ray.GetPoint(entry);
                m_StartCameraPosition = CameraPivot.position;
                m_IsMouseClick = true;
            }
            m_ScreenStartMousePosition = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
            if (m_Plane.Raycast(ray, out float entry))
            {
                // m_CurrentMousePosition = ray.GetPoint(entry);
                // m_NewPosition = CameraPivot.position + (m_StartMousePosition - m_CurrentMousePosition) ;
                //
                // m_NewPosition.y = Mathf.Clamp(m_NewPosition.y, MoveRangeY.m_Min, MoveRangeY.m_Max);
                // m_NewPosition.x = Mathf.Clamp(m_NewPosition.x, MoveRangeX.m_Min, MoveRangeX.m_Max);
                m_CurrentMousePosition = ray.GetPoint(entry);
                m_NewPosition = CameraPivot.position + (m_StartMousePosition - m_CurrentMousePosition) ;
                m_NewPosition = Quaternion.Inverse(CameraFlexRotation.rotation) * m_NewPosition;

                m_NewPosition.z = 0;
                m_NewPosition.y = Mathf.Clamp(m_NewPosition.y, MoveRangeY.m_Min + m_FixedRange.m_Min,
                    MoveRangeY.m_Max + m_FixedRange.m_Max);
                m_NewPosition.x = Mathf.Clamp(m_NewPosition.x, MoveRangeX.m_Min + m_FixedRange.m_Min,
                    MoveRangeX.m_Max + m_FixedRange.m_Max);

                if (Vector3.Distance(Input.mousePosition, m_ScreenStartMousePosition) > 0.01f && m_IsMouseClick)
                {
                    m_IsMouseClick = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (!m_IsMouseClick) return;

            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Cast ray");

            // Perform 2D raycast
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 100, WhatIsInteractObject);
            if (hits.Length > 0)
            {
                SelectableObject selectableObject = GetSelectableObject(hits);
                if (selectableObject != null)
                {
                    Vector3 screwPosition = selectableObject.GetCurrentScrewPosition();
                    Vector3 screwWorldPosition = MainCamera.WorldToScreenPoint(screwPosition);
                    Debug.Log("Got selectable object");
                    if (selectableObject.TryRemoveScrew(out ColorId colorId))
                    {
                        ScreenInGameContext.Events.SpawnScrew?.Invoke(screwWorldPosition, colorId);
                        
                    }
                }
            }
        }
    }

    private void HandleCameraZoom()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount != 2) return;
#endif
        if (Input.GetKey(KeyCode.Mouse0) && Input.touchCount == 2)
        {
            IsZooming = true;
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            if (Mathf.Abs(deltaMagnitudeDiff) > 5)
            {
                m_Size = ZoomTarget + deltaMagnitudeDiff * 0.04f;
                SetCameraSize(m_Size);
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            IsZooming = true;
            float size = MainCamera.orthographicSize - Input.mouseScrollDelta.y * 8f;
            SetCameraSize(size);
        }
    }
    private void MoveCamera()
    {
        //m_Velocity2 = 0;
        if (Mathf.Abs(MainCamera.orthographicSize - ZoomTarget) > 0.1f)
        {
            float newSize = Mathf.SmoothDamp(MainCamera.orthographicSize, ZoomTarget, ref m_Velocity2, SmoothZoomTime);
            MainCamera.orthographicSize = newSize;
            //UICamera.orthographicSize = newSize;
            RaycastCamera.orthographicSize = newSize * MoveIntensity;
        }
        
        if (Vector3.Distance(CameraPivot.position, m_NewPosition) > 0.1f)
        {
            CameraPivot.localPosition = Vector3.SmoothDamp(CameraPivot.localPosition, m_NewPosition, ref m_Velocity1, SmoothMoveTime);
        }
    }
    public void SetCameraSize(float size) 
    {
        ZoomTarget = Mathf.Clamp(size, ZoomRange.m_Min, ZoomRange.m_Max);
        MoveIntensity = MoveIntensityRange.Evaluate((ZoomTarget - ZoomRange.m_Min) / (ZoomRange.m_Max - ZoomRange.m_Min));
        
        m_FixedRange.m_Min = (ZoomTarget - ZoomRange.m_Min) * 1.2f;
        m_FixedRange.m_Max = (ZoomRange.m_Max - ZoomTarget) * 1.2f;
        m_NewPosition.y = Mathf.Clamp(m_NewPosition.y, MoveRangeY.m_Min + m_FixedRange.m_Min, MoveRangeY.m_Max + m_FixedRange.m_Max);
        m_NewPosition.x = Mathf.Clamp(m_NewPosition.x, MoveRangeX.m_Min + m_FixedRange.m_Min, MoveRangeX.m_Max + m_FixedRange.m_Max);
    }
    public void SaveLastCameraDistance()
    {
        m_LastSize = MainCamera.orthographicSize;
    }
    public void SetZoomOutMaxCamera()
    {
        SetCameraSize(ZoomRange.m_Max);
    }
    public void SetFocus(Transform target, float size)
    {
        m_NewPosition = Quaternion.Inverse(CameraFlexRotation.rotation) * target.position;
        SetCameraSize(size);
    }
    public void SetFocusIgnoreClamp(Vector3 target, float size)
    {
        m_NewPosition = Quaternion.Inverse(CameraFlexRotation.rotation) * target;
        SetCameraSizeIgnoreClamp(size);
    }
    public void SetFocusIgnoreClamp(Transform target, float size)
    {
        m_NewPosition = Quaternion.Inverse(CameraFlexRotation.rotation) * target.position;
        SetCameraSizeIgnoreClamp(size);
    }
    public void SetCameraSizeIgnoreClamp(float size) 
    {
        ZoomTarget = size;
    }

    public bool IsReachFocusPosition()
    {
        return Vector3.Distance(CameraPivot.localPosition, m_NewPosition) < 0.1f;
    }
    public void ResetZoomCamera()
    {
        SetCameraSize(m_LastSize);
    }

    private bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                m_IsTouchingUI = true;
                return true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && m_IsTouchingUI)
        {
            m_IsTouchingUI = false;
            return true;
        }

        return m_IsTouchingUI;
    }
    private static float GetVolume(Vector3 size)
    {
        return Mathf.Abs(size.x * size.y * size.z); 
    }
    public Vector3 GetCameraDirection()
    {
        return CameraPivot.position - MainCamera.transform.position;
    }
    private SelectableObject GetSelectableObject(RaycastHit2D[] hits)
    {
        SelectableObject interactObject = null;
        //float largestVolume = 0;

        foreach (var hit in hits)
        {
            SelectableObject interact = hit.collider.GetInteractObject();
            if (interact == null) continue;
            interactObject = interact;
            // float volume = GetVolume(hit.collider.bounds.size);
            // if (volume > largestVolume)
            // {
            //     largestVolume = volume;
            //     interactObject = interact;
            // }
        }

        return interactObject;
    }

    #region Creatives

    public List<SelectableObject> ListSelectableObjects = new();
    public List<CreativeCamStat> CamStats = new();
    public float moveTime;
    public float zoomTime;
    public float timeWaitZome;
    public float timeWaitTap;
    public float timeWaitZoomOutCam;
    
    [Button]
    private async UniTask MoveCamToPosition(int id)
    {
        CreativeCamStat camStat = CamStats[id];
        CameraPivot.gameObject.transform.DOMove(camStat.camPos, moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(async () =>
            {
                await UniTask.WaitForSeconds(timeWaitZome);
                DOTween.To(() => MainCamera.orthographicSize, x => MainCamera.orthographicSize = x, camStat.camSizeScrew,
                    zoomTime).OnComplete(async () =>
                {
                    // await UniTask.WaitForSeconds(timeWaitTap);
                    // Vector3 screwPosition = camStat.targetObject.GetCurrentScrewPosition();
                    // Vector3 screwWorldPosition = MainCamera.WorldToScreenPoint(screwPosition);
                    // Debug.Log("Got selectable object");
                    // if (camStat.targetObject.TryRemoveScrew(out ColorId colorId))
                    // {
                    //     ScreenInGameContext.Events.SpawnScrew?.Invoke(screwWorldPosition, colorId);
                    //     
                    // }
                    //
                    // await UniTask.WaitForSeconds(timeWaitZoomOutCam);
                    // DOTween.To(() => MainCamera.orthographicSize, x => MainCamera.orthographicSize = x, camStat.camSizeAnim,
                    //     zoomTime).OnComplete(() =>
                    // {
                    //     // CameraPivot.gameObject.transform.DOMove(camStat.camPos, moveTime)
                    //     //     .SetEase(Ease.Linear);
                    // });
                });
                //MainCamera.orthographicSize = camStat.camSize;
                //SetCameraSize(camStat.camSize);
            });
    }

    [Button]
    private async UniTask ResetCamToFloat(float size)
    {
        await UniTask.WaitForSeconds(timeWaitZome);
        DOTween.To(() => MainCamera.orthographicSize, x => MainCamera.orthographicSize = x, size,
            zoomTime);
    }
    // [Button]
    // private async UniTask ResetCam()
    // {
    //     await UniTask.WaitForSeconds(timeWaitZome);
    //     DOTween.To(() => MainCamera.orthographicSize, x => MainCamera.orthographicSize = x, 15.5f,
    //         zoomTime);
    // }
    [Button]
    private void TryRemoveScrew(Screw screw)
    {
        Vector3 screwPosition = screw.SelectableObject.GetCurrentScrewPosition();
        Vector3 screwWorldPosition = MainCamera.WorldToScreenPoint(screwPosition);
        if (screw.SelectableObject.TryRemoveScrew(out ColorId colorId))
        {
            ScreenInGameContext.Events.SpawnScrew?.Invoke(screwWorldPosition, colorId);
        }
    }

    #endregion
}

