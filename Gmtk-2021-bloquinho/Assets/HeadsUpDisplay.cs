using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject _holder = null;
    private GameObject Holder => _holder;

    [Header("Jump")]
    [SerializeField]
    private GameObject jumpUI = null;
    private GameObject JumpUI => jumpUI;

    [SerializeField]
    private Image _jumpIcon = null;
    private Image JumpIcon => _jumpIcon;

    [SerializeField]
    private TextMeshProUGUI _jumpQtd = null;
    private TextMeshProUGUI JumpQtd => _jumpQtd;

    [SerializeField]
    private Image _jumpKey = null;
    private Image JumpKey => _jumpKey;

    [Header("Shooter")]
    [SerializeField]
    private GameObject dashUI = null;
    private GameObject DashUI => dashUI;

    [SerializeField]
    private Image _dashIcon = null;
    private Image DashIcon => _dashIcon;

    [SerializeField]
    private TextMeshProUGUI _dashQtd = null;
    private TextMeshProUGUI DashQtd => _dashQtd;

    [SerializeField]
    private Image _dashKey = null;
    private Image DashKey => _dashKey;


    [Header("Shooter")]
    [SerializeField]
    private GameObject shooterUI = null;
    private GameObject ShooterUI => shooterUI;

    [SerializeField]
    private Image _shooterIcon = null;
    private Image ShooterIcon => _shooterIcon;

    [SerializeField]
    private TextMeshProUGUI _shooterQtd = null;
    private TextMeshProUGUI ShooterQtd => _shooterQtd;

    [SerializeField]
    private Image _shooterKey = null;
    private Image ShooterKey => _shooterKey;

    public static HeadsUpDisplay Instance { get; private set; }

    private bool ShowingHud { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        Holder.gameObject.SetActive(false);
        JumpUI.gameObject.SetActive(false);
        DashUI.gameObject.SetActive(false);
        ShooterUI.gameObject.SetActive(false);

        ShowingHud = false;
    }

    public void UpdateJump(bool show, int qtd = 1)
    {
        if (!show)
        {
            JumpUI.gameObject.SetActive(false);
            return;
        }

        JumpQtd.text = "x" + qtd;

        float alpha = qtd < 1 ? 0.3f : 1.0f;

        JumpIcon.color = new Color(JumpIcon.color.r, JumpIcon.color.g, JumpIcon.color.b, alpha);
        JumpKey.color = new Color(JumpKey.color.r, JumpKey.color.g, JumpKey.color.b, alpha);

        JumpUI.gameObject.SetActive(true);

        if (!ShowingHud)
        {
            Holder.gameObject.SetActive(true);
            ShowingHud = true;
        }
    }

    public void UpdateDash(bool show, int qtd = 1)
    {
        if (!show)
        {
            DashUI.gameObject.SetActive(false);
            return;
        }

        DashQtd.text = "x" + qtd;

        float alpha = qtd < 1 ? 0.3f : 1.0f;

        DashIcon.color = new Color(DashIcon.color.r, DashIcon.color.g, DashIcon.color.b, alpha);
        DashKey.color = new Color(DashKey.color.r, DashKey.color.g, DashKey.color.b, alpha);

        DashUI.gameObject.SetActive(true);

        if (!ShowingHud)
        {
            Holder.gameObject.SetActive(true);
            ShowingHud = true;
        }
    }

    public void UpdateLagolas(bool show, int qtd = 1)
    {

    }

    public void UpdateShooter(bool show, int qtd = 1)
    {
        if (!show)
        {
            ShooterUI.gameObject.SetActive(false);
            return;
        }

        ShooterQtd.text = "x" + qtd;

        float alpha = qtd < 1 ? 0.3f : 1.0f;

        ShooterIcon.color = new Color(ShooterIcon.color.r, ShooterIcon.color.g, ShooterIcon.color.b, alpha);
        ShooterKey.color = new Color(ShooterKey.color.r, ShooterKey.color.g, ShooterKey.color.b, alpha);

        ShooterUI.gameObject.SetActive(true);

        if (!ShowingHud)
        {
            Holder.gameObject.SetActive(true);
            ShowingHud = true;
        }
    }

}
