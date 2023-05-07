using Ink.Runtime;
using UnityEngine;

public class DialogManager : MonoBehaviour{
    [SerializeField] private TextAsset storyJson;
    [SerializeField] private DialogCanvas dialogCanvas;
    private Story story;

    public static DialogManager Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SetStory();
    }

    private void SetStory(){
        story = new Story(storyJson.ToString());
    }

    public void GetCharacterDialog(string characterName){
        if(DialogCanvas.IsToggling){return;}
        if (!DialogCanvas.Active){
            dialogCanvas.ToggleDialogCanvas(true);
            story.ChoosePathString(characterName);
        }
        if (story.canContinue){
            dialogCanvas.ShowDialog(story.Continue());
            return;
        }
        dialogCanvas.ToggleDialogCanvas(false);
    }

    private void SaveStory(){
        GameSaveSystem.SaveData.storyJson = story.state.ToJson();
    }

    private void LoadStory(){
        string savedStory = GameSaveSystem.SaveData.storyJson;
        if (string.IsNullOrEmpty(savedStory)){ return; }
        story.state.LoadJson(savedStory);
    }
}


// public class DialogManager : MonoBehaviour
// {
//     public static int GameDay;
//     public static GameObject TalkingNPC;
//     [SerializeField] private TextAsset dialogJson;
//     [SerializeField] private Text dialogText;
//     [SerializeField] private Text nameText;
//     private Canvas myCanvas;
//     private float enableTime;
//     private Movement playerMovement;
//     private StringBuilder charName = new StringBuilder();
//     private StringBuilder charLine = new StringBuilder();
//     public Story story { get; private set; }
//     private static StoryData storyData;
//     private static string myGuid;
//     private bool displayingChoices;
//     public static StoryVariableStates VariableStates { get; private set; } = new StoryVariableStates();
//     public event EventHandler VariablesChanged;
//     public static event Action<Story> OnCreateStory;
//     public static bool DialogOpened { get; private set; }
//
//     private void Awake()
//     {
//         SaveManager.SaveAllData += SaveStoryData;
//         myGuid = GetComponent<UniqueId>().Id;
//         choicesCanvas.Setup();
//         myCanvas = GetComponent<Canvas>();
//         storyData = SaveManager.GetData<StoryData>(myGuid);
//         if (storyData == null)
//             storyData = new StoryData();
//         GameDay = storyData.GameDay;
//         VariableStates = storyData.variableStates;
//         SetStory(dialogJson);
//         //ContinueStory();
//     }
//
//     private void Start()
//     {
//         playerMovement = Globals.Player.GetComponent<Movement>();
//     }
//
//     public void SetStory(TextAsset storyJson)
//     {
//         if (story != null && story.state != null)
//         {
//             SaveStoryState();
//         }
//
//         story = new Story(storyJson.text);
//         BindMethods();
//         ObserveVariables();
//         OnCreateStory?.Invoke(story);
//
//         if (story != null && !String.IsNullOrEmpty(storyData.jsonStory))
//         {
//             LoadStoryState();
//         }
//     }
//
//     private void SaveStoryData()
//     {
//         storyData.GameDay = GameDay;
//         storyData.variableStates = VariableStates;
//         SaveStoryState();
//         SaveManager.SaveData(myGuid, storyData);
//     }
//
//     public void SaveStoryState()
//     {
//         storyData.jsonStory = story.state.ToJson();
//     }
//
//     public void LoadStoryState()
//     {
//         story.state.LoadJson(storyData.jsonStory);
//     }
//
//     public void ChangeStoryVariable<T>(string variableName, T value)
//     {
//         story.variablesState[variableName] = value;
//     }
//
//     public void ContinueStory()
//     {
//         if (displayingChoices) return;
//
//         string[] separator = { ": " };
//         charLine.Clear();
//         charName.Clear();
//
//         if (story.canContinue)
//         {
//             story.Continue();
//             while (!HasContent(story.currentText) && story.canContinue)
//             {
//                 story.Continue();
//             }
//
//             if (HasContent(story.currentText))
//             {
//                 string[] newLine = story.currentText.Split(separator, 2, System.StringSplitOptions.None);
//                 charName.Append(newLine[0]);
//                 charLine.Append("    ");
//                 charLine.Append(newLine[1]);
//
//                 bool activateName = (charName.ToString() == "JOGO" || charName.ToString() == "Jogo") ? false : true;
//                 nameText.transform.parent.gameObject.SetActive(activateName);
//
//                 nameText.text = charName.ToString();
//                 dialogText.text = charLine.ToString();
//             }
//             else
//             {
//                 CloseDialog();
//             }
//
//         }
//         else if (story.currentChoices.Count > 0)
//         {
//             displayingChoices = true;
//             string[] choices = story.currentChoices.Select(text => text.text).ToArray();
//             choicesCanvas.DisplayChoices(choices);
//         }
//         else
//         {
//             CloseDialog();
//         }
//     }
//
//     public void MakeChoice(int index)
//     {
//         story.ChooseChoiceIndex(index);
//         displayingChoices = false;
//         ContinueStory();
//     }
//
//     private bool HasContent(string line)
//     {
//         if (!String.IsNullOrEmpty(line) && !String.IsNullOrWhiteSpace(line))
//         {
//             return true;
//         }
//         else return false;
//     }
//
//     public void ContinueStoryOnTap(LeanFinger finger)
//     {
//         if (Time.time - finger.Age > enableTime)
//         {
//             ContinueStory();
//         }
//     }
//
//     public void StartDialog(string inkKnot)
//     {
//         Globals.DialogManager.OpenDialog();
//         Globals.DialogManager.JumpTo(inkKnot);
//     }
//
//     public void JumpTo(string inkKnot)
//     {
//         story.ChoosePathString(inkKnot);
//         ContinueStory();
//     }
//
//     private void BindMethods()
//     {
//         story.BindExternalFunction("PauseTimeline", () => { Globals.CutsceneManager.PauseTimeline(); });
//         story.BindExternalFunction("ResumeTimeline", () => { Globals.CutsceneManager.ResumeTimeline(); });
//         story.BindExternalFunction("newQuest", (string questName) => { Globals.QuestManager.StartNewQuest(questName); });
//         story.BindExternalFunction("RemoveQuest", (string questName) => { Globals.QuestManager.RemoveQuest(questName); });
//         story.BindExternalFunction("Debug", (string value) => { Debug.Log(value); });
//         story.BindExternalFunction("ChooseCutscene", (int choiceIndex) => { Globals.CutsceneManager.ChooseCutscene(choiceIndex); });
//         story.BindExternalFunction("PlayCutscene", (int cutsceneNum) => { CutsceneManager.TriggerCutscene(cutsceneNum); });
//         story.BindExternalFunction("CloseDialog", () => { CloseDialog(); });
//         story.BindExternalFunction("ChangeGameDay", (int day) => { GameDay++; story.variablesState["changedDay"] = true; });
//         story.BindExternalFunction("ChangeDayTime", (string time) => { Player.ChangeDayTime((DayTime)Enum.Parse(typeof(DayTime), time)); });
//         story.BindExternalFunction("SetCutscenePlayable", (string cutsceneNum) => { Globals.CutsceneManager.SetCutscenePlayable(cutsceneNum); });
//         story.BindExternalFunction("SetPlayerAnimatorBool", (string parameter, bool value) => { Player.animationControl.anim.SetBool(parameter, value); });
//     }
//
//     private void ObserveVariables()
//     {
//         story.ObserveVariable("chopTask", (string varName, object value) => { VariableStates.chopTask = (bool)value; NotifyVariableChange(); });
//         story.ObserveVariable("waterTask", (string varName, object value) => { VariableStates.waterTask = (bool)value; NotifyVariableChange(); });
//         story.ObserveVariable("chickenTask", (string varName, object value) => { VariableStates.chickenTask = (bool)value; NotifyVariableChange(); });
//     }
//
//     public void NotifyVariableChange()
//     {
//         VariablesChanged?.Invoke(this, EventArgs.Empty);
//     }
//
//     public void OpenDialog()
//     {
//         enableTime = Time.time;
//         for (int i = 0; i < transform.childCount - 2; i++)
//         {
//             transform.GetChild(i).gameObject.SetActive(true);
//         }
//         myCanvas.enabled = true;
//         DialogOpened = true;
//         //playerMovement.canMove = false;
//     }
//
//     public void CloseDialog()
//     {
//         SaveStoryState();
//         for (int i = 0; i < transform.childCount - 2; i++)
//         {
//             transform.GetChild(i).gameObject.SetActive(false);
//         }
//         myCanvas.enabled = false;
//         DialogOpened = false;
//         //playerMovement.canMove = true;
//
//         if (TalkingNPC != null)
//         {
//             //TalkingNPC.GetComponent<NPC_Movement>().enabled = true;
//             TalkingNPC.GetComponent<NPC_Movement>().enabled = false;
//             TalkingNPC = null;
//         }
//     }
// }
//
// public class StoryData : ObjectData
// {
//     public int GameDay;
//     public string jsonStory;
//     public StoryVariableStates variableStates;
//
//     public StoryData()
//     {
//         GameDay = 1;
//         variableStates = new StoryVariableStates();
//     }
// }
//
// [Serializable]
// public class StoryVariableStates
// {
//     public bool chopTask;
//     public bool waterTask;
//     public bool chickenTask;
// }