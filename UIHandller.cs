using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandller : MonoBehaviour
{
    public float displayTime = 4.0f;
    // breyta sem segir hversu lengi skilaboðin verða uppi á skjánum
    private VisualElement m_NonPlayerDialogue;
    // breyta sem vísar á skilaboðin (skilaboð búin til í unity editor)
    private float m_TimerDisplay;
    // breyta sem verður gerð að teljara fyrir tíman sem skilaboðin eiga að vera uppi

    private VisualElement m_HealthBar;
    // sækjum tilvísun í helth bar visual elementið

    public static UIHandller instance {get; private set;}

    private void Awake()
    {
        instance = this;
        // birtum visual elementið
    }

    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        // sækjum tilvísun í ui documentið
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);
        // tengjum healthbar elementið við health bar visual elementið í unity

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialog");
        // sækjum skilaboðin sem við ætlum að birta
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        // gerum skilaboðin ósýnileg meðan ekki er búið að instantiata þau
        m_TimerDisplay = -1.0f;
        // setjum teljarann á -1 þannig að hann byrji ekki að telja niður strax

    }

    private void Update()
   {
        if (m_TimerDisplay > 0)
        {
           m_TimerDisplay -= Time.deltaTime;
           // ef að teljarinn er hærri en 0 þá er 1 sekúnda dregin frá honum, þetta er í uptade falli þannig að talið er niður á sekúntu fresti
           if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
                // ef að teljarinn fet undir 0 verða skilaboðinn ekki lengur birt
            }
       }
   }

    public void SetHealthValue (float percentage)
    {
        m_HealthBar.style.width = Length.Percent(100 * percentage);
        // lengd heathl barins fer eftir hversu mörg líf leikmaður hefur
    }

    public void DisplayDialogue()
   {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
        // þegar kallað er á fallið DisplayDialogue() verða skilaboðin birt og teljarinn settur í gang
   }
}

