using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Shaper2D
{
	[ExecuteInEditMode]
	public class Shaper2D:MonoBehaviour{

		public Color innerColor;
		private Color _innerColor;
		
		public Color outerColor;
		private Color _outerColor;
		
		[Range(3,100)]
		public int sectorCount=36;
		private int _sectorCount;
		
		[Range(1,360)]
		public float arcDegrees=360;
		private float _arcDegrees;
		
		[Range(0,360)]
		public float rotation=90;
		private float _rotation;
		
		public float innerRadius=1.5f;
		private float _innerRadius;
		
		public float outerRadius=2f;
		private float _outerRadius;
		
		[Range(0f,1f)]
		public float starrines=0f;
		private float _starrines;

		[HideInInspector]
		public ColType colliderType=ColType.None;
		private ColType _colliderType;

		[HideInInspector]
		public int sortingLayer=0;
		private int _sortingLayer;

		[HideInInspector]
		public int orderInLayer=0;
		private int _orderInLayer=0;

		private float _radiusPlus;
		private bool _isCanvas=false;
		private float _a;
		private CanvasRenderer _cr;
		private MeshFilter _mf;
		private MeshRenderer _mr;
		
		[HideInInspector]
		public Material useMaterial=null;
		
		private Mesh _mesh;
		private List<Vector3> _vertices=new List<Vector3>(200);
		private List<Vector3> _uvs=new List<Vector3>(200);
		private List<Color> _colors=new List<Color>(200);
		private int[] _triangles=new int[0];
		public int TriangleCount => _triangles.Length/3;
		private bool IsDirty
		{
			get
			{
				var dirty = false;
				if (_innerColor != innerColor)
				{
					_innerColor = innerColor;
					dirty = true;
				}
				if (_outerColor != outerColor)
				{
					_outerColor = outerColor;
					dirty = true;
				}
				if (_sectorCount != sectorCount)
				{
					_sectorCount = sectorCount;
					dirty = true;
				}
				if (_arcDegrees != arcDegrees)
				{
					_arcDegrees = arcDegrees;
					dirty = true;
				}
				if (_rotation != rotation)
				{
					_rotation = rotation;
					dirty = true;
				}
				if (_outerRadius != outerRadius)
				{
					_outerRadius = outerRadius;
					dirty = true;
				}
				if (_innerRadius != innerRadius)
				{
					_innerRadius = innerRadius;
					dirty = true;
				}
				if (_starrines != starrines)
				{
					_starrines = starrines;
					dirty = true;
				}
				if (_sortingLayer != sortingLayer)
				{
					_sortingLayer = sortingLayer;
					dirty = true;
				}
				if (_orderInLayer != orderInLayer)
				{
					_orderInLayer = orderInLayer;
					dirty = true;
				}
				return dirty;
			}
		}
		
		void Awake()
		{
			if (useMaterial == null)
			{
				useMaterial=(Material)Resources.Load("Shaper2DMaterial",typeof(Material));
			}
			if(_isCanvas != IsChildOfCanvas(transform))
			{
				_isCanvas=!_isCanvas;
				TakeCareOfComponents();
			}
			if(innerColor.a == 0)
			{
				float hue=Random.Range(0f,1f);
				while((hue*360f)>=236f && (hue*360f)<=246f){
					hue=Random.Range(0f,1f);
				}
				float saturation=Random.Range(0.9f,1f);
				innerColor=Color.HSVToRGB(hue,saturation/2,1f);
				outerColor=innerColor;
			}
			TakeCareOfComponents();
			ApplyChangesIfAny();
		}

		void Update()
		{
			if (useMaterial == null)
			{
				useMaterial=(Material)Resources.Load("Shaper2DMaterial",typeof(Material));
			}
			#if UNITY_EDITOR
			Tools.pivotMode=PivotMode.Pivot;
			if(transform.hasChanged)
			{
				if(_isCanvas != IsChildOfCanvas(transform))
				{
					_isCanvas=!_isCanvas;
					TakeCareOfComponents();
				}
			}
			#endif
			ApplyChangesIfAny();
			transform.hasChanged = false;
		}

		private void TakeCareOfComponents()
		{
			if(_isCanvas)
			{
				DestroyImmediate(GetComponent<MeshFilter>());
				DestroyImmediate(GetComponent<MeshRenderer>());
				if(GetComponent<CanvasRenderer>()==null) gameObject.AddComponent<CanvasRenderer>();
				_cr=GetComponent<CanvasRenderer>();
				_cr.SetMaterial(useMaterial,null);
			}
			else
			{
				DestroyImmediate(GetComponent<CanvasRenderer>());
				if(GetComponent<MeshFilter>()==null) gameObject.AddComponent<MeshFilter>();
				if(GetComponent<MeshRenderer>()==null) gameObject.AddComponent<MeshRenderer>();
				_mf=GetComponent<MeshFilter>();
				_mr=GetComponent<MeshRenderer>();
				_mr.sharedMaterial=useMaterial;
				sortingLayer=_mr.sortingLayerID;
				orderInLayer=_mr.sortingOrder;
			}
		}
		
		public void ForceApplyChanges()
		{
			ApplyChangesIfAny(true);
		}

		private void ApplyChangesIfAny(bool force=false)
		{
			if (!IsDirty && !transform.hasChanged && !force)
			{
				return;
			}
			//Dont allow radius to be negative
			innerRadius = Mathf.Max(0, innerRadius);
			outerRadius = Mathf.Max(0, outerRadius);
			//Don't allow inner radius to be bigger than outer radius
			outerRadius = Mathf.Max(innerRadius + 0.01f, outerRadius);
			innerRadius = Mathf.Min(outerRadius - 0.01f, innerRadius);
			//When generating a star, we only allow even number of sectors larger or equal than 6
			if (starrines > 0)
			{
				sectorCount = Mathf.Max(6, sectorCount);
				sectorCount += sectorCount % 2 == 0 ? 0 : 1;
			}
			GenerateMesh();
			if(_isCanvas)
			{
				if (_cr == null) _cr = GetComponent<CanvasRenderer>();
				if (_cr == null) TakeCareOfComponents();
				_cr.SetMesh(_mesh);
			}
			else
			{
				if (_mf == null) _mf = GetComponent<MeshFilter>();
				if (_mf == null) TakeCareOfComponents();
				_mf.sharedMesh = _mesh;
			}
			if (_mr == null)
			{
				return;
			}
			_mr.sortingLayerID = sortingLayer;
			_mr.sortingOrder = orderInLayer;
		}
		
		private bool IsChildOfCanvas(Transform t){
			if (t.GetComponent<Canvas>() != null)
			{
				return true;
			}
			if (t.parent != null)
			{
				return IsChildOfCanvas(t.parent);
			}
			return false;
		}

		private void GenerateMesh()
		{
			if(_mesh==null)
			{
				_mesh=new Mesh();
				_mesh.name="Shaper2D";
			}
			_mesh.Clear();
			if(innerRadius==0)
			{
				GenerateCircle();
			}
			else
			{
				GenerateArc();
			}
			if (colliderType != ColType.None)
			{
				UpdateCollider();
			}
		}

		public Mesh GetMesh()
		{
			return _mesh;
		}

		public void UpdateCollider(){
			Collider2D col=GetComponent<Collider2D>();
			if(col!=null){
				Vector2[] cpoints=new Vector2[_vertices.Count];
				for(int i=0;i<_vertices.Count;i++){
					cpoints[i]=(Vector2)_vertices[i];
				}
				if(col.GetType()==typeof(UnityEngine.PolygonCollider2D)){
					GetComponent<PolygonCollider2D>().points=cpoints;
				}else if(col.GetType()==typeof(UnityEngine.EdgeCollider2D)){
					GetComponent<EdgeCollider2D>().points=cpoints;
				}
			}
		}

		void GenerateCircle()
		{
			_vertices.Clear();
			_colors.Clear();
			_radiusPlus = 0f;
			int realSectorCount = sectorCount;
			if (arcDegrees != 360f) realSectorCount++;
			for (int i = 0; i < realSectorCount; i++)
			{
				_a = (((arcDegrees / sectorCount) * i + rotation) * Mathf.Deg2Rad);
				if (starrines > 0)
				{
					if (i % 2 == 0) _radiusPlus = (outerRadius * starrines);
					else _radiusPlus = -(outerRadius * starrines);
				}
				_vertices.Add(new Vector2(
					(float)(Mathf.Cos(_a) * (outerRadius + _radiusPlus)),
					(float)(Mathf.Sin(_a) * (outerRadius + _radiusPlus))
				));
				_colors.Add(outerColor);
			}
			_vertices.Add(Vector3.zero);
			_colors.Add(innerColor);
			_mesh.SetVertices(_vertices);
			_mesh.SetColors(_colors);
			SetUVs();
			int trianglesNum = _vertices.Count - 1; //-1 because last point is center
			if (arcDegrees != 360f) trianglesNum--; //Downt join the ends
			if (_triangles == null || _triangles.Length != trianglesNum * 3) _triangles = new int[trianglesNum * 3];
			for (int i = 0; i < trianglesNum; i++)
			{
				_triangles[(i * 3) + 0] = (i + 1 == _vertices.Count - 1 ? 0 : i + 1);
				_triangles[(i * 3) + 1] = i;
				_triangles[(i * 3) + 2] = _vertices.Count - 1;
			}
			_mesh.SetTriangles(_triangles, 0);
			_mesh.RecalculateNormals();
			_mesh.RecalculateBounds();
		}

		void GenerateArc()
		{
			_vertices.Clear();
			_colors.Clear();
			_radiusPlus = 0f;
			for (int i = 0; i < (sectorCount + 1); i++)
			{
				_a = ((((arcDegrees / ((sectorCount + 1) - 1)) * i)) + rotation) * Mathf.Deg2Rad;
				if (starrines > 0)
				{
					if (i % 2 == 0) _radiusPlus = (outerRadius * starrines);
					else _radiusPlus = -(outerRadius * starrines);
				}

				_vertices.Add(new Vector3(
					(float)(Mathf.Cos(_a) * (outerRadius + _radiusPlus)),
					(float)(Mathf.Sin(_a) * (outerRadius + _radiusPlus))
				));
				_colors.Add(outerColor);
			}

			for (int i = (sectorCount + 1) - 1; i >= 0; i--)
			{
				_a = ((arcDegrees / ((sectorCount + 1) - 1)) * i) + rotation;
				if (starrines > 0)
				{
					if (i % 2 == 0) _radiusPlus = (innerRadius * starrines);
					else _radiusPlus = -(innerRadius * starrines);
				}

				_vertices.Add(new Vector3(
					(float)(Mathf.Cos(_a * Mathf.Deg2Rad) * (innerRadius + _radiusPlus)),
					(float)(Mathf.Sin(_a * Mathf.Deg2Rad) * (innerRadius + _radiusPlus))
				));
				_colors.Add(innerColor);
			}

			_mesh.SetVertices(_vertices);
			_mesh.SetColors(_colors);
			SetUVs();
			if (_triangles == null || _triangles.Length != sectorCount * 6) _triangles = new int[sectorCount * 6];
			for (int i = 0; i < sectorCount; i++)
			{
				//First triangle
				_triangles[(i * 6) + 0] = i + 1;
				_triangles[(i * 6) + 1] = i;
				_triangles[(i * 6) + 2] = _vertices.Count - (i + 1);
				//Second triangle
				_triangles[(i * 6) + 3] = i + 1;
				_triangles[(i * 6) + 4] = _vertices.Count - (i + 1);
				_triangles[(i * 6) + 5] = _vertices.Count - (i + 2);
			}
			_mesh.SetTriangles(_triangles, 0);
			_mesh.RecalculateNormals();
			_mesh.RecalculateBounds();
		}

		void SetUVs()
		{
			_uvs.Clear();
			for (int i = 0; i < _vertices.Count; i++)
			{
				_uvs.Add(Vector3.zero);
			}
			_mesh.SetUVs(0, _uvs);
		}

		public enum ColType
		{
			None,
			Polygon,
			Edge
		}
	}
}
