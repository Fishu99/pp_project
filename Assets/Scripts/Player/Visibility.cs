using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour{

    public class ViewCastInfo{
        public bool hit;
        public Vector3 point;
        public Vector3 normal;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, Vector3 _normal, float _dst, float _angle){
            hit = _hit;
            point = _point;
            normal = _normal;
            dst = _dst;
            angle = _angle;
        }

        public ViewCastInfo(){}
    }

    public class EdgeInfo{
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB){
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    [SerializeField]
    float viewRadius;

    [SerializeField]
    [Range(0,380)]
    float viewAngle;

    [SerializeField]
    float meshResolution;

    [SerializeField]
    int edgeResolveIterations;

    [SerializeField]
    float edgeDstThreshold;

    [SerializeField]
    LayerMask targetMask;

    [SerializeField]
    LayerMask obstacleMask;

    [SerializeField]
    MeshFilter meshFilter;

    [SerializeField]
    float maskCutawayDst = .1f;

    [SerializeField]
    Vector3 offset;

    Mesh viewMesh;
    List<Transform> visibleTargets = new List<Transform>();

    void Start(){
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;

        //StartCoroutine(FindTargetsWithDelay(.2f));
    }

    void LateUpdate(){
        DrawVisibility();
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    public void FindVisibleTargets(){
        visibleTargets.Clear();
        Collider [] targetsInViewRadius = Physics.OverlapSphere(transform.position + offset, viewRadius, targetMask);
        for(int i = 0; i < targetsInViewRadius.Length; i++){
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dir = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dir) < viewAngle / 2){
                float dst = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dir, dst, obstacleMask)){
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal){
        if(!angleIsGlobal){
            angle += transform.eulerAngles.x;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0,Mathf.Cos(angle*Mathf.Deg2Rad));
    }

    void DrawVisibility(){
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        List<Vector3> viewNormals = new List<Vector3>();
        ViewCastInfo oldViewCastInfo = new ViewCastInfo();
        for(int i = 0; i < stepCount; i++){
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize *i;
            ViewCastInfo newViewCastInfo = ViewCast(angle);

            if(i > 0){
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCastInfo.dst - newViewCastInfo.dst) > edgeDstThreshold;
                if(oldViewCastInfo.hit != newViewCastInfo.hit || (oldViewCastInfo.hit && newViewCastInfo.hit && edgeDstThresholdExceeded)){
                    EdgeInfo edge = FindEdge(oldViewCastInfo,newViewCastInfo);
                    if(edge.pointA != Vector3.zero){
                        viewPoints.Add(edge.pointA);
                    }else{
                        viewPoints.Add(edge.pointB);
                    }
                    viewNormals.Add(Vector3.Lerp(newViewCastInfo.normal, oldViewCastInfo.normal, 0.5f));
                }
            }

            viewPoints.Add(newViewCastInfo.point);
            viewNormals.Add(newViewCastInfo.normal);
            oldViewCastInfo = newViewCastInfo;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2)*3];

        vertices[0] = Vector3.zero;
        for(int i =0; i < vertexCount -1; i++){
            Vector3 localPos = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i+1] = localPos + localPos.normalized * maskCutawayDst;

            if(i < vertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast){
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i =0; i <edgeResolveIterations; i++){
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if(newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded){
                minAngle = angle;
                minPoint = newViewCast.point;
            }else{
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint,maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle){
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position + offset, dir, out hit, viewRadius, obstacleMask)){
            return new ViewCastInfo(true, hit.point, hit.normal, hit.distance, globalAngle);
        }else{
            return new ViewCastInfo(false, transform.position + offset + dir * viewRadius, Vector3.zero , viewRadius, globalAngle);
        }
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
}
