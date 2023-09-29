using Godot;

using GodotInk;

using System;

public partial class ShowText : Control
{
    [Export]
    private Control root;

    [Export]
    private InkStory story;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        story.Continued += Story_Continued;
        story.Continue();
    }

    private void Story_Continued()
    {
        GD.Print($"step: text={story.CurrentText.Trim()}, cancont={story.CanContinue}, numchoices={story.CurrentChoices.Count}");
        if (!string.IsNullOrEmpty(story.CurrentText))
        {
            var textNode = new Label { Text = story.CurrentText, HorizontalAlignment = HorizontalAlignment.Center };
            root.AddChild(textNode);
        }
        if (story.CurrentChoices.Count > 0)
        {
            var choiceContainer = new HBoxContainer() { Alignment = BoxContainer.AlignmentMode.Center };
            choiceContainer.SetAnchorsPreset(LayoutPreset.CenterTop);
            foreach (var choice in story.CurrentChoices)
            {
                CreateChoiceButton(choice, choiceContainer);
            }
            root.AddChild(choiceContainer);
        }
        if (story.CanContinue)
        {
            var continueButton = new Button() { Text = "Continue" };
            continueButton.Pressed += () =>
            {
                story.Continue();
                continueButton.QueueFree();
            };
            root.AddChild(continueButton);
        }
    }

    private void CreateChoiceButton(InkChoice choice, Container container)
    {
        var button = new Button { Text = choice.Text };
        container.AddChild(button);
        button.Pressed += () =>
        {
            story.ChooseChoiceIndex(choice.Index);
            story.Continue();
            container.QueueFree();
        };
    }
}
