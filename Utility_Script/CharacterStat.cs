using Godot;
using System;
using System.Collections;

public partial class CharacterStat : Node
{
    public enum AffectStamina
    {
        Rest, Nap, Sleep, Walk, Run
    }
    public enum StatVar
    {
        Health, Mood, Stamina
    }
    public enum ActionType
    {
        Attacked, Heal
    }
    public enum Operation
    {
        Add, Substract, Multiply
    }
    //For each class inherit this class, set a default value
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxStamina { get; private set; }
    public float CurrentStamina { get; private set; }
    public float MaxMood { get; private set; }
    public float CurrentMood { get; private set; }

    public override void _Ready()
    {
        base._Ready();
    }
    protected virtual void LoadSave(string saveDataName)
    {
        //load from save file the stat here
        //Create a new dictionary, save to it
    }
    public void ModHealth(float modifier, ActionType action)
    {
        switch (action)
        {
            case ActionType.Attacked:
                CurrentHealth -= modifier;
                break;
            case ActionType.Heal:
                CurrentHealth += modifier;
                break;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
        checkDeath();
    }
    public void ModMaxStat(float modifier, Operation operation, StatVar var)
    {
        //this is to increase max possible health/stamina/mood, which is achieved via items, level upgrade, etc.
        switch (var)
        {
            case StatVar.Health:
                MaxHealth = Mathf.Max(1f, performOperation(modifier, operation, MaxHealth));
                break;
            case StatVar.Stamina:
                MaxStamina =  Mathf.Max(1f, performOperation(modifier, operation, MaxStamina));
                break;
            case StatVar.Mood:
                MaxMood =  Mathf.Max(1f, performOperation(modifier, operation, MaxMood));
                break;
        }
    }
    private void checkDeath()
    {
        if (CurrentHealth <= 0f)
        {
            //trigger change of state
            GD.Print("Perished");
        }
    }
    public void ResetStat(StatVar var)
    {
        switch (var)
        {
            case StatVar.Health:
                CurrentHealth = MaxHealth;
                break;
            case StatVar.Stamina:
                CurrentStamina = MaxStamina;
                break;
            case StatVar.Mood:
                CurrentMood = MaxMood;
                break;
        }
    }
    private float performOperation(float modifier, Operation operation, float var)
    {
        switch (operation)
        {
            case Operation.Multiply:
                var *= modifier;
                break;
            case Operation.Add:
                var += modifier;
                break;
            case Operation.Substract:
                var -= modifier;
                break;
        }
        return var;
    }
}
