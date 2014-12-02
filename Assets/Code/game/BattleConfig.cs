using System.Xml;
using engine;
using UnityEngine;

public class BattleConfig  {

    public static float scope = 5;
    public static float everyTime = 0.02f;

    public static int monsterTestNum;     //怪物测试数量
    public static float viewRange;          //视野范围
    public static float attackRange;        //攻击范围
    public static float sprintRange;        //冲刺范围
    public static float minSprintRange;     //冲刺最小检测范围
    public static float hitEffectTime;      //闪白时间
    public static float sprintSpeed;        //冲刺速度
    public static float sprintShadowTime;   //残影时间
    public static float monsterShakeRange;  //怪物震屏幅度
    public static float monsterShakeNum;		//怪物震屏数量
    public static float shakeMFrequency; 	//震屏的速率，频率
    public static float farNearRange;       //相机远近振幅
    public static float leftRightRange;     //相机左右振幅
    public static float upDownRange;        //相机上下振幅
    public static float frontBackRange;     //相机前后振幅
    public static float radiusRange;        //相机半径振幅
    public static float zoomSpeed;          //增进速度
    public static float zoomToBoss;         //boss拉近时间
    public static float zommOutBoss;        //boss拉远时间   
    public static float cameraNearType;     //相机拉近的曲线类型
    public static float cameraNearTime;     //拉近的时间
    public static float cameraFarType;      //拉远的曲线类型
    public static float cameraFarTime;      //拉远的时间
    public static float maxFieldOfView;     //最大的振幅
	
    public static float hitBackTime;
    public static int hitBackType;
    public static float hitBackFreezeTime;
    //怪物击退配置
    public static float hitBackTime1;
    public static float hitBackTime2;
    public static float hitBackPoint1;
    public static float hitBackPoint2;
    public static float hitBackPoint3;
    //震屏配置
    public static float cameraShakeTime1;
    public static float cameraShakeTime2;
    public static float cameraShakePoint1;
    public static float cameraShakePoint2;
    public static float cameraShakePoint3;

    //冲刺屏幕缓动延迟时间
    public static float rushDelay;


    //放慢效果
    public static float slowTimeScale;
    public static float slowDuration;
    public static float slowCD;
    
    //boss出生镜头设计
    //public static Vector3 petPos;
    //public static float winCameraView;
    //public static float winHeight;
    //public static float lookPos;

    //pet askhelp/flee speed
    public static float petFleeSpeedRate=1f;
    public static float petHelpSpeedRate = 1f;
    public static float petFleeCD = 5;
    public static float petTryHelpTime = 10;
    public static float petFleeDistance = 10;
    public static float petFleedDistanceSqr;
    public static float petFriendNumberRate=1;
    public static float fleeSuccessStayTime;

    //怪物当前攻击目标不是攻击自身目标，受到多少次攻击后改变攻击目标
    public static int monsterChangeTargetThreshhold = 3;

    public static void init(){
          XmlDocument xml = new XmlDocument();
          xml.LoadXml(App.res.loadText("Local/data/xml/battleConfig"));
          XmlNodeList list = xml.GetElementsByTagName("common");
        for (int i = 0, max = list.Count; i < max; i++) {
            XmlElement e = list.Item(i) as XmlElement;
            string id=e.GetAttribute("ID");
            if(id=="1000000"){
                monsterTestNum=Utility.toInt(e.GetAttribute("monsterTestNum"));
                viewRange=Utility.toFloat(e.GetAttribute("viewRange"));
                attackRange=Utility.toFloat(e.GetAttribute("attackRange"));
                sprintRange = Utility.toFloat(e.GetAttribute("sprintRange"));
                minSprintRange = Utility.toFloat(e.GetAttribute("minSprintRange"));
                rushDelay = Utility.toFloat(e.GetAttribute("sprintCameraDelay"));
            }else if(id=="1000100"){
                hitEffectTime=Utility.toFloat(e.GetAttribute("hitEffectTime"));
                sprintSpeed=Utility.toFloat(e.GetAttribute("sprintSpeed"));
                sprintShadowTime=Utility.toFloat(e.GetAttribute("sprintShadowTime"));
                   
            }else if(id=="1000200"){
                upDownRange=Utility.toFloat(e.GetAttribute("upDownRange"));
                leftRightRange=Utility.toFloat(e.GetAttribute("leftRightRange"));
                frontBackRange=Utility.toFloat(e.GetAttribute("frontBackRange"));
                radiusRange=Utility.toFloat(e.GetAttribute("radiusRange"));
                zoomSpeed=Utility.toFloat(e.GetAttribute("zoomSpeed"));
                zoomToBoss=Utility.toFloat(e.GetAttribute("zoomToBoss"));
                zommOutBoss=Utility.toFloat(e.GetAttribute("zommOutBoss"));

            }else if(id=="1000300"){
                cameraNearType=Utility.toFloat(e.GetAttribute("cameraNearType"));
                cameraNearTime=Utility.toFloat(e.GetAttribute("cameraNearTime"));
                cameraFarType=Utility.toFloat(e.GetAttribute("cameraFarType"));
                cameraFarTime=Utility.toFloat(e.GetAttribute("cameraFarTime"));
                farNearRange=Utility.toFloat(e.GetAttribute("farNearRange"));
                maxFieldOfView=Utility.toFloat(e.GetAttribute("maxFieldOfView"));

                cameraShakeTime1 = Utility.toFloat(e.GetAttribute("cameraShakeTime1"));
                cameraShakeTime2 = Utility.toFloat(e.GetAttribute("cameraShakeTime2"));
                cameraShakePoint1 = Utility.toFloat(e.GetAttribute("cameraShakePoint1"));
                cameraShakePoint2 = Utility.toFloat(e.GetAttribute("cameraShakePoint2"));
                cameraShakePoint3 = Utility.toFloat(e.GetAttribute("cameraShakePoint3"));
             
            }else if(id=="1000400"){
                monsterShakeRange=Utility.toFloat(e.GetAttribute("monsterShakeRange"));
                monsterShakeNum=Utility.toFloat(e.GetAttribute("monsterShakeNum"));
                shakeMFrequency=Utility.toFloat(e.GetAttribute("shakeMFrequency"));
                hitBackTime=Utility.toFloat(e.GetAttribute("hitBackTime"));
                hitBackType=Utility.toInt(e.GetAttribute("hitBackType"));
                hitBackFreezeTime = Utility.toFloat(e.GetAttribute("hitBackFreezeTime"));

                slowTimeScale = Utility.toFloat(e.GetAttribute("slowTimeScale"));
                slowDuration = Utility.toFloat(e.GetAttribute("slowDuration"));
                slowCD = Utility.toFloat(e.GetAttribute("slowCD"));
            }
            else if (id == "1000500"){
                hitBackTime1 = Utility.toFloat(e.GetAttribute("hitBackTime1"));
                hitBackTime2 = Utility.toFloat(e.GetAttribute("hitBackTime2"));
                hitBackPoint1 = Utility.toFloat(e.GetAttribute("hitBackPoint1"));
                hitBackPoint2 = Utility.toFloat(e.GetAttribute("hitBackPoint2"));
                hitBackPoint3 = Utility.toFloat(e.GetAttribute("hitBackPoint3"));
            }
            else if (id == "2000000")
            {
                //petPos = Utility.toVector3(e.GetAttribute("petPos"));
                //winCameraView = Utility.toFloat(e.GetAttribute("winView"));
                //winHeight = Utility.toFloat(e.GetAttribute("winHeight"));
                //lookPos = Utility.toFloat(e.GetAttribute("lookPos"));
            }
            else if (id == "2000001")
            {
                petFleeSpeedRate = Utility.toFloat(e.GetAttribute("petFleeSpeedRate"));
                petHelpSpeedRate = Utility.toFloat(e.GetAttribute("petHelpSpeedRate"));
                petFleeCD = Utility.toFloat(e.GetAttribute("petFleeCD"));
                petTryHelpTime = Utility.toFloat(e.GetAttribute("petTryHelpTime"));
                petFriendNumberRate = Utility.toFloat(e.GetAttribute("petFriendNumberRate"));
                petFleeDistance = Utility.toFloat(e.GetAttribute("petFleeDistance"));
                petFleedDistanceSqr = petFleeDistance * petFleeDistance;

                fleeSuccessStayTime = Utility.toFloat(e.GetAttribute("fleeSuccessStayTime"));
            }
            else if (id == "2000002")
            {
                monsterChangeTargetThreshhold = Utility.toInt(e.GetAttribute("monsterChangeTargetThreshhold"));
            }
        }
        

    }
}
