using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDBattle : MonoBehaviour
{
   public TextMeshProUGUI namaText;
   public Slider sliderHp;
   public TextMeshProUGUI type;

   public void setHUD (Unit unit){
      namaText.text = unit.Nama;
      sliderHp.maxValue = unit.MaxHp;
      sliderHp.value = unit.CurrentHp;
      type.text = unit.type;
   }

   public void setHp(int hp){
      sliderHp.value = hp;
   }
}
