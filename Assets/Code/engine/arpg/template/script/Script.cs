using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using engine;
/// <summary>
/// 剧情模版
/// </summary>

public class ScriptStepSingle
{
    public string type;
    public string talk;

    public void read(XmlElement e) {
        type = e.GetAttribute("type");
        talk = e.GetAttribute("talk");
    }
}

public class ScriptStep {
    public int charId;
    public string prefab;
    public List<ScriptStepSingle> talks;

    public void read(System.Xml.XmlElement e) {
        charId = Utility.toInt(e.GetAttribute("charId"));
        prefab = e.GetAttribute("prefab");
        talks = new List<ScriptStepSingle>();
        XmlNodeList list = e.ChildNodes;
        int length = list.Count;
        for (var i = 0; i < length; i++) {
            XmlElement ele = list.Item(i) as XmlElement;
            ScriptStepSingle single = new ScriptStepSingle();
            single.read(ele);
            talks.Add(single);
        }
    }
}

public class Script : BaseTemp {
    public List<ScriptStep> steps;

    public override void read(System.Xml.XmlElement e)
    {
        base.read(e);
        XmlNodeList list = e.ChildNodes;
        int length = list.Count;
        steps = new List<ScriptStep>();
        for (var i = 0; i < length; i++) {
            XmlElement ele = list.Item(i) as XmlElement;
            ScriptStep step = new ScriptStep();
            step.read(ele);
            steps.Add(step);
        }
    }

}
