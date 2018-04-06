using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Rendering.Animation
{
    class DancingBall : Feature
    {
         public DancingBall()
        {
            Name = "Dancing Ball";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.AnimationCategoryId;
        }

         private float heightOfObject = 0; //记录小球的高度
         private float timerOfObject = 0;  //记录运动时间
         private float speedOfObject = 60; //起始速度
         private float xspeed = 10;        //X方向的速度
         private float distanceX = -125;   //X方向的位移
         private SceneNode m_Object;       //小球的节点
         private FaceStyle fs = null;
         private AnyCAD.Presentation.RenderWindow3d m_RenderView;
         private Random random = new Random();

         private void DancingBall_RenderTick()
         {
             timerOfObject += 0.5f;

             //z方向的位移
             heightOfObject = speedOfObject * timerOfObject - (9.8f * timerOfObject * timerOfObject) * 0.5f;
             //x方向的位移
             distanceX += xspeed;
             if (heightOfObject <= 0.0f)
             {
                 distanceX -= xspeed;
                 timerOfObject = 0;
                 xspeed = -xspeed;
                 heightOfObject = 10;
             }

             //设置位移矩阵
             Matrix4 trf = GlobalInstance.MatrixBuilder.MakeTranslate(new Vector3(distanceX, 0, heightOfObject));
             float scaleValue = heightOfObject*0.01f + 1;
             Matrix4 scale = GlobalInstance.MatrixBuilder.MakeScale(new Vector3(scaleValue, scaleValue, scaleValue));
             m_Object.SetTransform(trf*scale);


             // Change color          
             fs.SetColor((int)(random.NextDouble()*100), (int)(random.NextDouble()*100), (int)(random.NextDouble()*100));

             m_RenderView.RequestDraw();
         }


         public override bool Run(FeatureContext context)
         {
             if (m_Object == null)
             {
                 TopoShape sphere = GlobalInstance.BrepTools.MakeSphere(Vector3.ZERO, 10);
                 fs = new FaceStyle();
                 m_Object = context.ShowGeometry(sphere);
                 m_Object.SetFaceStyle(fs);
             }
             else
             {
                 context.ShowSceneNode(m_Object);
             }

             m_RenderView = context.RenderView;
             m_RenderView.RenderTick += new AnyCAD.Presentation.RenderEventHandler(DancingBall_RenderTick);
             return false;
         }

         public override void OnExit(FeatureContext context)
         {
             m_RenderView.RenderTick -= new AnyCAD.Presentation.RenderEventHandler(DancingBall_RenderTick);
             m_RenderView = null;
         }
    }
}
