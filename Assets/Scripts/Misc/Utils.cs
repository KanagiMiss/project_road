using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	//这部分代码实现上位于Utils类之外
	public enum BoundsTest {
		center,    //游戏对象的中心是否位于屏幕中？
		onScreen,  //游戏对象是否完全位于屏幕之中？
		offScreen  //游戏对象是否完全位于屏幕之外？
	}

	public class Utils : MonoBehaviour {

		//========================Bounds函数======================\\
		//接受两个Bounds类型变量，并返回包含这两个Bounds的新Bounds
		public static Bounds BoundsUnion(Bounds b0, Bounds b1){
			if (b0.size == Vector3.zero && b1.size != Vector3.zero) {
				return b1;
			} else if (b0.size != Vector3.zero && b1.size == Vector3.zero) {
				return b0;
			} else if (b0.size == Vector3.zero && b1.size == Vector3.zero) {
				return b0;
			}
			//b0和b1都不为0
			b0.Encapsulate (b1.min);  //Encapsulate是形成囊状物，封装的意思
			b0.Encapsulate (b1.max);
			return b0;
		}

		//将一个GameObject的所有组件的边界都框在一个Bounds中返回
		public static Bounds CombineBoundsOfChildren(GameObject go){
			//创建一个空白Bounds变量b
			Bounds b = new Bounds(Vector3.zero,Vector3.zero);
			//如果游戏对象具有渲染器组件
			if(go.GetComponent<Renderer>() != null){
				b = BoundsUnion (b, go.GetComponent<Renderer>().bounds);
			}
			//如果游戏对象具有碰撞器组件
			if(go.GetComponent<Collider>() != null){
				b = BoundsUnion (b, go.GetComponent<Collider>().bounds);
			}
			//递归遍历游戏对象go的Transform组件的每一个子组件，并且将它们的游戏对象都包含进来
			foreach(Transform t in go.transform){
				b = BoundsUnion (b, CombineBoundsOfChildren(t.gameObject));
			}
			return b;
		}


		//静态局部字段，在camBounds属性定义中使用
		static private Bounds _camBounds;
		//创建一个静态只读全局属性camBounds
		static public Bounds camBounds{
			get{ 
				//
				if(_camBounds.size == Vector3.zero){
					//使用默认摄像机设置调用SetCameraBounds()
					SetCameraBounds ();
				}
				return _camBounds;
			}
		}


		//使用Camera的左上角和右下角来初始化_camBounds的值
		public static void SetCameraBounds(Camera cam = null){
			//如果未传入任何cam，则使用main camera
			if (cam == null) {
				cam = Camera.main;
			}
			//对摄像机做一些重要的假设
			//1.摄像机为正投影摄像机
			//2.摄像机的旋转为R:[0,0,0]
			Vector3 topLeft = new Vector3 (0, 0, 0);
			Vector3 bottomRight = new Vector3 (Screen.width, Screen.height, 0);
			//将topLeft和bottomRight转换为世界坐标
			//不是太懂底下的操作，近端和远端什么的...
			Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft); //（Near）近端左上角
			Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);//(Far)远端右下角
			boundTLN.z += cam.nearClipPlane;
			boundBRF.z += cam.farClipPlane;
			//查找边界框的中心
			Vector3 center = (boundTLN + boundBRF) / 2f;
			_camBounds = new Bounds (center, Vector3.zero); //初始化_camBounds
			//将_camBounds扩展到包含左上角和右下角
			_camBounds.Encapsulate (boundTLN);
			_camBounds.Encapsulate (boundBRF);
		}


		//检查边界框bnd是否位于镜头的边界框camBounds之内
		public static Vector3 ScreenBoundsCheck(Bounds bnd, 
			BoundsTest test = BoundsTest.center){
			return BoundsInBoundsCheck (camBounds, bnd, test);
		}

		//根据test参数，依据不同的标准来检查边界框lilB是否在bigB中
		//如果test通过，则返回Vector3(0,0,0)，不然返回的是一个可以调整lilB通过test的一个offset向量
		public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB,
			BoundsTest test = BoundsTest.onScreen){

			//获取边界框lilB的中心
			Vector3 pos = lilB.center;
			Vector3 off = Vector3.zero;

			switch (test) {
			case BoundsTest.center:
				//当test参数值为center时，需要确认lilB的center是否在bigB中
				//如果不在，off需要将center调整到bigB之内
				if (bigB.Contains (pos))  //test通过
					return Vector3.zero;
				//test不通过，需要调整，分别检查x/y/z坐标
				if (pos.x > bigB.max.x) {
					//x坐标超了
					off.x = pos.x - bigB.max.x;
				} else if (pos.x < bigB.min.x) {
					off.x = pos.x - bigB.min.x;
				}
				if (pos.y > bigB.max.y) {
					//y坐标超了
					off.y = pos.y - bigB.max.y;
				} else if (pos.y < bigB.min.y) {
					off.y = pos.y - bigB.min.y;
				}
				if (pos.z > bigB.max.z) {
					//z坐标超了
					off.z = pos.z - bigB.max.z;
				} else if (pos.z < bigB.min.z) {
					off.z = pos.z - bigB.min.z;
				}
				return off;
			case BoundsTest.onScreen:
				//如果test参数值为onScreen，函数要判断是否lilB整体在bigB内
				//如果不在，则off需要将lilB调整到bigB中
				if (bigB.Contains (lilB.min) && bigB.Contains (lilB.max)) {//test通过
					return Vector3.zero;
				}
				//test不通过，需要调整
				if (lilB.max.x > bigB.max.x) {
					off.x = lilB.max.x - bigB.max.x;
				} else if (lilB.min.x < bigB.min.x) {
					off.x = lilB.min.x - bigB.min.x;
				}
				if (lilB.max.y > bigB.max.y) {
					off.y = lilB.max.y - bigB.max.y;
				} else if (lilB.min.y < bigB.min.y) {
					off.y = lilB.min.y - bigB.min.y;
				}
				if (lilB.max.z > bigB.max.z) {
					off.z = lilB.max.z - bigB.max.z;
				} else if (lilB.min.z < bigB.min.z) {
					off.z = lilB.min.z - bigB.min.z;
				}
				return off;
			case BoundsTest.offScreen:
				//如果test的参数是offScreen，则需要lilB的一部分在bigB中，也就是得有交集
				if (bigB.Contains (lilB.max) || bigB.Contains (lilB.min)) {//test通过
					return Vector3.zero;
				}
				//test未通过
				if (lilB.min.x > bigB.max.x) {
					off.x = lilB.min.x - bigB.max.x;
				} else if (lilB.max.x < bigB.min.x) {
					off.x = lilB.max.x - bigB.min.x;
				}
				if (lilB.min.y > bigB.max.y) {
					off.y = lilB.min.y - bigB.max.y;
				} else if (lilB.max.y < bigB.min.y) {
					off.y = lilB.max.y - bigB.min.y;
				}
				if (lilB.min.z > bigB.max.z) {
					off.z = lilB.min.z - bigB.max.z;
				} else if (lilB.max.z < bigB.min.z) {
					off.z = lilB.max.z - bigB.min.z;
				}
				return off;
			}
			return Vector3.zero;
		}

		//如果go的tag不是“Untagged”，则返回这个go对象，如果是，则递归查找transform的父对象的go
		//如果go的transform组件没有父对象了，则返回null
		public static GameObject FindTaggedParent(GameObject go){
			if (go.tag != "Untagged") {
				return go;
			}
			//go并不满足条件

			if (go.transform.parent == null) {//go已经是对象树的顶层了
				return null;
			}
			//沿着树的边向上遍历
			return FindTaggedParent (go.transform.parent.gameObject);
		}

		public static GameObject FindTaggedParent(Transform t){
			return FindTaggedParent (t.gameObject);
		}

		//=======================材质函数===========================
		//用一个List返回一个GameObject所有的材质
		static public Material[] GetAllMaterials(GameObject go) {
			List<Material> mats = new List<Material> ();
			if (go.GetComponent<Renderer> () != null) {
				mats.Add (go.GetComponent<Renderer> ().material);
			}
			foreach (Transform t in go.transform) {
				mats.AddRange (GetAllMaterials(t.gameObject));
			}
			return mats.ToArray ();
		}
	}

}