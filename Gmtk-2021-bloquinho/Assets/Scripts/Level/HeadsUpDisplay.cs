using Block;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
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
            Reset();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            UpdateJump(0);
            UpdateDash(0);
            UpdateShooter(0);
        }

        public void UpdateBlockStatus(BaseBlock baseBlock, int quantity)
        {
            switch (baseBlock)
            {
                case DashBlock _:
                    UpdateDash(quantity);
                    break;
                case JumpBlock _:
                    UpdateJump(quantity);
                    break;
                case ShootingBlock _:
                    UpdateShooter(quantity);
                    break;
            }
        }

        private void UpdateJump(int qtd)
        {
            JumpQtd.text = "x" + qtd;

            var alpha = GetAlphaForBlockUI(qtd);

            JumpIcon.color = new Color(JumpIcon.color.r, JumpIcon.color.g, JumpIcon.color.b, alpha);
            JumpKey.color = new Color(JumpKey.color.r, JumpKey.color.g, JumpKey.color.b, alpha);

            JumpUI.gameObject.SetActive(true);
        }

        private void UpdateDash(int qtd)
        {
            DashQtd.text = "x" + qtd;

            var alpha = GetAlphaForBlockUI(qtd);

            DashIcon.color = new Color(DashIcon.color.r, DashIcon.color.g, DashIcon.color.b, alpha);
            DashKey.color = new Color(DashKey.color.r, DashKey.color.g, DashKey.color.b, alpha);

            DashUI.gameObject.SetActive(true);
        }

        private void UpdateShooter(int qtd)
        {
            ShooterQtd.text = "x" + qtd;

            var alpha = GetAlphaForBlockUI(qtd);

            ShooterIcon.color = new Color(ShooterIcon.color.r, ShooterIcon.color.g, ShooterIcon.color.b, alpha);
            ShooterKey.color = new Color(ShooterKey.color.r, ShooterKey.color.g, ShooterKey.color.b, alpha);

            ShooterUI.gameObject.SetActive(true);
        }

        private static float GetAlphaForBlockUI(int qtd) => qtd < 1 ? 0.3f : 1.0f;
    }
}
