/************************************************************
 * 该文件实现的功能、编写者、编写日期
 * function:血条的组件
 * author:ljx
 * create date:2014-09-29
****************************************************************/

using System.Collections;
using UnityEngine;

namespace engine {
    public class HpBar : MonoBehaviour {
        public UISprite backGround;
        public UISprite topForeGround;
        public UISprite foreGround;
        public UISprite topHuaWen;

        private int _maxHp;
        private int _currentHp;
        private bool _hpDirty = true;
        private Vector3 _scale;
        private float _maxWidth;
        private Character _owner;
        private float _hitTime;
        private float _yOffset;
        private bool _stopUpdate = false;

        public void setVisible(bool visible, bool stopUpdate = false) {
            _stopUpdate = stopUpdate;
            if (visible) {
                topForeGround.alpha = 1;
                foreGround.alpha = 1;
                backGround.alpha = 1;
                topHuaWen.alpha = 1;
            }
            else {
                topForeGround.alpha = 0;
                foreGround.alpha = 0;
                backGround.alpha = 0;
                topHuaWen.alpha = 0;
            }
        }

        private void Awake() {
            _scale = new Vector3(1, 1, 1);
            _currentHp = 0;
            _maxHp = 0;
            _hitTime = 0;
            _hpDirty = true;
            _maxWidth = topForeGround.width;
            updateHp();
        }


        private void LateUpdate()
        {
            if (_stopUpdate)
                return;
            if(_owner == null || _owner.model == null ||_owner.isBoss()) {
                Destroy(Parent);
                return;
            }
            if (_hitTime > 3f && !_owner.isPlayer() && !_owner.isHero()) {
                Parent.SetActive(false);
                return;
            }
            //setVisible(true);
            //if(_hpDirty)
            resetPos();
         
            _hitTime += Time.deltaTime;
        }

        //刷新显示UI
        private void updateHp() {
            if (topForeGround.type == UISprite.Type.Filled) {
                float fill;
                if (_maxHp == 0)
                    fill = 1;
                else
                    fill = (float) _currentHp/_maxHp;
                topForeGround.fillAmount = fill;
                if (fill < 1)
                    displayHpEffect(_currentHp);
            }
            else if (topForeGround.type == UISprite.Type.Sliced) {
                float fill;
                if (_maxHp == 0)
                    fill = 1;
                else
                    fill = (float) _currentHp/_maxHp;
                if (Utility.judgeFloatZero(fill)) {
                    topForeGround.alpha = 0;
                    foreGround.alpha = 0;
                }
                else if (fill <= 1) {
                    int finalW = (int) (_maxWidth*fill);
                    topForeGround.width = finalW;
                    if (topForeGround.width <= topForeGround.minWidth) {
                        float lesW = topForeGround.minWidth/_maxWidth; //剩余比例值
                        float scaleX = fill/lesW; //需要缩放的值
                        if (scaleX <= 0) scaleX = 0;
                        _scale = topForeGround.transform.localScale;
                        _scale.x = scaleX;
                        foreGround.alpha = 0;
                    }
                    else {
                        _scale.x = 1; //复原缩小值
                    }
                    topForeGround.transform.localScale = _scale;
                    displayHpEffect(finalW);
                }
            }
        }

        private void displayHpEffect(int finalW) {
            if (foreGround.type == UISprite.Type.Filled) {
                float fill = (float) finalW/_maxHp;
                float currentFill = foreGround.fillAmount;
                Hashtable ht = iTween.Hash("from", currentFill, "to", fill, "time", 0.5f, "easeType",
                    iTween.EaseType.easeInCubic, "onupdate", "changeHpBar");
                iTween.ValueTo(gameObject, ht);
            }
            else if (foreGround.type == UISprite.Type.Sliced) {
                float currentW = foreGround.width;
                Hashtable ht = iTween.Hash("from", currentW, "to", finalW, "time", 0.5f, "easeType",
                    iTween.EaseType.easeInCubic, "onupdate", "changeHpBar");
                iTween.ValueTo(gameObject, ht);
            }
        }

        private void changeHpBar(float finalValue) {
            if (foreGround.type == UISprite.Type.Filled)
                foreGround.fillAmount = finalValue;
            else
                foreGround.width = (int) finalValue;
            if (finalValue == 0) Parent.SetActive(false);
        }

        //getter and setter
        public Character Owner {
            set {
                _owner = value;
                //_yOffset =1.7f;//temp 
                Transform trans = _owner.getTagPoint("help_hp");
                _yOffset = trans != null ? trans.localPosition.y / trans.localScale.y : 1.4f;
                _currentHp = _owner.data.hp;
                _maxHp = _owner.data.maxhp; 
                if( _owner.isPlayer())topForeGround.spriteName = "HP_player";
                else if (_owner.isHero()) topForeGround.spriteName = "HP_hero";
                else {
                    foreGround.spriteName = "HP_monster";
                    topForeGround.spriteName = "HP_monster1";
                    topHuaWen.gameObject.SetActive(false);
                    transform.localScale = new Vector3(54f/86f,6f/10f,1);
                    if (_owner.isBoss())
                    {
                        Parent.SetActive(false);
                    }
                }   
            }
        }

        public int CurrentHp {
            set {
                _hpDirty = true;
                _currentHp = value;
                _hitTime = 0f;
                if (!Parent.activeSelf)
                    Parent.SetActive(true);
                updateHp();
            }
        }

        public GameObject Parent {
            get { return transform.parent.gameObject; }
        }

        public bool HPDirty
        {
            set
            {
                if (_hpDirty != value)
                {
                    _hpDirty = value;
                    if (!_hpDirty)
                        resetPos();  
                }                           
            }
        }

        public void resetPos()
        {
            if (Camera.main == null)
            {
                setVisible(false);
                return;
            }
            setVisible(true);
            Transform trans = transform.parent;
            trans.localScale = Vector3.one;
            Vector3 uiPot =
                Camera.main.WorldToScreenPoint(new Vector3(_owner.Position.x, _owner.Position.y + _yOffset,
                    _owner.Position.z));
            float fRatio = (float)(Screen.width) / (Screen.height);
            if (fRatio > 1.0f)
            {
                float bench = Screen.width / (float)Screen.height * 320.0f;
                float diff = uiPot.x - (Screen.width * 0.5f);
                float ratio = bench / (Screen.width * 0.5f);
                float x = ratio * diff;
                float diffy = uiPot.y - Screen.height * 0.5f;
                float ratioy = 320.0f / (Screen.height * 0.5f);
                float y = ratioy * diffy;
                trans.localPosition = new Vector3(x, y, 0f);
            }
            else
            {
                float benchy = Screen.height / (float)Screen.width * 480.0f;
                float diffy = uiPot.y - (Screen.height * 0.5f);
                float ratioy = benchy / (Screen.height * 0.5f);
                float y = ratioy * diffy;
                float diffx = uiPot.x - (Screen.width * 0.5f);
                float ratiox = 480.0f / (Screen.width * 0.5f);
                float x = ratiox * diffx;
                trans.localPosition = new Vector3(x, y, 0f);
            } 
        }
    }
}
