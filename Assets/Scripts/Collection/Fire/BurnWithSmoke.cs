using UnityEngine;

public class BurnWithSmoke : Burn
{
    [Header("Smoke Settings")]
    public ParticleSystem smokeParticleSystem;  // 煙霧的粒子系統
    public GameObject magicCircle;              // 魔法陣物件

    private int originalEmissionRate;           // 原始的 emission 數量
    private ParticleSystem.EmissionModule smokeEmission;

    private void Start()
    {
        // 確保有指定煙霧粒子系統
        if (smokeParticleSystem != null)
        {
            smokeEmission = smokeParticleSystem.emission;
            originalEmissionRate = (int)smokeEmission.rateOverTime.constant;
        }
        else
        {
            Debug.LogWarning("Smoke ParticleSystem not assigned!");
        }

        // 確保魔法陣一開始是隱藏的
        if (magicCircle != null)
        {
            magicCircle.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (smokeParticleSystem != null)
        {
            // 減少煙霧 Emission 數量
            int currentRate = (int)smokeEmission.rateOverTime.constant;
            int newRate = Mathf.Max(0, currentRate - (originalEmissionRate / 4));

            smokeEmission.rateOverTime = newRate;

            // 檢查是否煙霧完全停止
            if (newRate <= 0)
            {
                ShowMagicCircle();
            }
        }
    }

    private void ShowMagicCircle()
    {
        if (magicCircle != null)
        {
            magicCircle.SetActive(true);

            // 可以在這裡添加魔法陣出現時的特效
            // 例如：漸顯效果、粒子特效等
        }
    }
}