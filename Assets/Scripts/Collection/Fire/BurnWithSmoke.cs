using UnityEngine;

public class BurnWithSmoke : Burn
{
    [Header("Smoke Settings")]
    public ParticleSystem smokeParticleSystem;  // �������ɤl�t��
    public GameObject magicCircle;              // �]�k�}����

    private int originalEmissionRate;           // ��l�� emission �ƶq
    private ParticleSystem.EmissionModule smokeEmission;

    private void Start()
    {
        // �T�O�����w�����ɤl�t��
        if (smokeParticleSystem != null)
        {
            smokeEmission = smokeParticleSystem.emission;
            originalEmissionRate = (int)smokeEmission.rateOverTime.constant;
        }
        else
        {
            Debug.LogWarning("Smoke ParticleSystem not assigned!");
        }

        // �T�O�]�k�}�@�}�l�O���ê�
        if (magicCircle != null)
        {
            magicCircle.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (smokeParticleSystem != null)
        {
            // ��ַ��� Emission �ƶq
            int currentRate = (int)smokeEmission.rateOverTime.constant;
            int newRate = Mathf.Max(0, currentRate - (originalEmissionRate / 4));

            smokeEmission.rateOverTime = newRate;

            // �ˬd�O�_������������
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

            // �i�H�b�o�̲K�[�]�k�}�X�{�ɪ��S��
            // �Ҧp�G����ĪG�B�ɤl�S�ĵ�
        }
    }
}