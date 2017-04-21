using UnityEngine;
using UnityEngine.UI;

namespace Code.Menus
{
    public class Menu : MonoBehaviour
    {
        public static Menu Instance;

        public RectTransform MenuWindow;            // Set in editor
        public Text MenuText;                       // Set in editor

        public RectTransform CharacterContentPanel; // Set in editor
        public RectTransform InventoryContentPanel; // Set in editor
        public RectTransform WorldMapContentPanel;  // Set in editor
        public RectTransform HouseContentPanel;     // Set in editor
        public RectTransform DiplomacyContentPanel; // Set in editor
        public RectTransform IntrigueContentPanel;  // Set in editor
        public RectTransform WarsContentPanel;      // Set in editor
        public RectTransform JournalContentPanel;   // Set in editor

        private RectTransform _currentTab;

        private bool _active;

        // Use this for initialization
        void Start()
        {
            _currentTab = InventoryContentPanel;
            SetActiveTabCharacter();
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenWindow()
        {
            if (_active) return;
            _active = true;
            MenuWindow.gameObject.SetActive(true);
        }

        public void CloseWindow()
        {
            _active = false;
            MenuWindow.gameObject.SetActive(false);
        }

        public void SetActiveTabCharacter()
        {
            SetActiveTab(CharacterContentPanel, "Character");
        }

        public void SetActiveTabInventory()
        {
            SetActiveTab(InventoryContentPanel, "Inventory");
        }

        public void SetActiveTabWorldMap()
        {
            SetActiveTab(WorldMapContentPanel, "WorldMap");
        }

        public void SetActiveTabHouse()
        {
            SetActiveTab(HouseContentPanel, "House"); // Todo: Name of house
        }

        public void SetActiveTabDiplomacy()
        {
            SetActiveTab(DiplomacyContentPanel, "Diplomacy");
        }

        public void SetActiveTabIntrigue()
        {
            SetActiveTab(IntrigueContentPanel, "Intrigue");
        }

        public void SetActiveTabWars()
        {
            SetActiveTab(WarsContentPanel, "Wars");
        }

        public void SetActiveTabJournal()
        {
            SetActiveTab(JournalContentPanel, "Journal");
        }

        private void SetActiveTab(RectTransform tab, string tabText)
        {
            if (tab == _currentTab) return;
            _currentTab.gameObject.SetActive(false);
            tab.gameObject.SetActive(true);
            _currentTab = tab;

            MenuText.text = tabText;
        }
    }
}
