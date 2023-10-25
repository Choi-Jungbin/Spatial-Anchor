using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpatialAnchor
{
    public class UpdatePlay : MonoBehaviour
    {
        [SerializeField] PlayManager playManager;
        [SerializeField] TextMeshProUGUI ammo;
        [SerializeField] TextMeshProUGUI score;

        void OnEnable()
        {
            ammo.text = "0";
            score.text = "0";
        }

        // Update is called once per frame
        void Update()
        {
            ammo.text = playManager.ammo.ToString();
            score.text = playManager.score.ToString();
        }
    }
}
