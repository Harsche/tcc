using Ink.Runtime;
using UnityEngine;

public class Character : MonoBehaviour{
    [SerializeField] private string characterName;
    [SerializeField] private string storyJson;
    private Story story;
    
    
    public void SetStory(){
        story = new Story(storyJson);
    }

    private void GetStoryLine(){
        while (story.canContinue) {
            Debug.Log (story.Continue ());
        }
    }
}