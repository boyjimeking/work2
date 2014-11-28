using UnityEngine;
using System.Collections;
using engine;

public class OtherPlayerData : CharData {
    public int sex;
    public int career;
    public void read(ByteBuffer buffer) {
         id = buffer.readInt();
         name = buffer.readUTF();
         sex = buffer.readByte();
         career = buffer.readByte();
         hp = buffer.readInt();
         moveSpeed = buffer.readFloat();

         this.charTemplate = App.template.getTemp<CharTemplate>(getTemplateId());
    }
    protected int getTemplateId() {
        return 1;//TODO use sex and career
    }
}
