using UnityEngine;
using UnityEngine.InputSystem;

public class SwingScript : MonoBehaviour
{
    [Header("Swing Variables")]
    [SerializeField] private Transform startSwingHand;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask swingableLayer;
    [SerializeField] private LineRenderer webRenderer;
    [SerializeField] private float maxDistance;
    [SerializeField] private float pullingStrength = 500f;

    [Header("Prediction Point")]
    [SerializeField] private Transform predictionPoint;

    [Header("Input Actions")]
    [SerializeField] private InputActionProperty swingAction;

    private Vector3 swingPoint;
    private SpringJoint joint;
    private bool bHasHit;
    private bool bIsSwinging;
    
    private void Update()
    {
        GetSwingPoint();

        if (swingAction.action.WasPressedThisFrame())
        {
            StartSwing();
        }
        else if(swingAction.action.WasReleasedThisFrame())
        {
            ReleaseSwing();
        }

        PullRope();
        DrawWeb();
    }

    public void StartSwing()
    {
        if (bHasHit)
        {
            joint = rb.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distance = Vector3.Distance(rb.position, swingPoint);
            joint.maxDistance = distance;

            joint.spring = 4.5f;
            joint.damper = 7;
            joint.massScale = 4.5f;

            bIsSwinging = true;

            PullRope();
        }
    }

    public void PullRope()
    {
        if (!joint)
            return;

        Vector3 direction = joint.connectedAnchor - transform.position;
        rb.AddForce(direction.normalized * pullingStrength * Time.deltaTime, ForceMode.Impulse);

        float distance = Vector3.Distance(rb.position , swingPoint);
        joint.maxDistance = distance;     
    }

    public void ReleaseSwing()
    {
        bIsSwinging = false;
        Destroy(joint);
    }

    private void GetSwingPoint()
    {
        if (joint)
        {
            predictionPoint.gameObject.SetActive(false);
            return;
        }

        RaycastHit hit;

        bHasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out hit, maxDistance, swingableLayer);

        if (bHasHit)
        {
            swingPoint = hit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
    }

    public void DrawWeb()
    {
        if (!joint)
        {
            webRenderer.enabled = false;
        }
        else
        {
            webRenderer.enabled = true;
            webRenderer.positionCount = 2;
            webRenderer.SetPosition(0, startSwingHand.position);
            webRenderer.SetPosition(1, swingPoint);
        }
    }
}
