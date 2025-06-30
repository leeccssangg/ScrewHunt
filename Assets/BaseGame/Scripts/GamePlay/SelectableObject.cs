using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
using Spine.Unity;
using TW.Reactive.CustomComponent;
using TW.Utility.CustomComponent;
using UnityEngine;

public class SelectableObject : ACachedMonoBehaviour
{
    [field: SerializeField] public List<Screw> Screws { get; private set; } = new List<Screw>();
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public ReactiveValue<int> CurrentScrewCount { get; private set; }
    [field: SerializeField] public SkeletonAnimation Animation { get; private set; }
    
    
    public void Init(int id)
    {
        this.Id = id;
        foreach (var screw in Screws)
        {
            screw.Init(this);
        }

        if (Screws.Count > 0)
        {
            CurrentScrewCount.Value = Screws.Count;
            CurrentScrewCount.ReactiveProperty.Subscribe(OnScrewsCountChanged).AddTo(this);
        }
        PlayIdleAnimation();
    }
    public void PlayIdleAnimation()
    {
        // Implement idle animation logic here
        if (Animation == null) return;
        Animation.AnimationState.SetAnimation(0, "action1", true);
    }
    public void PlayRemoveScrewAnimation()
    {
        // Implement remove screw animation logic here
    }
    public void PlayRemoveAllScrewsAnimation()
    {
        // Implement remove all screws animation logic here
        //OnRemoveAllScrews();
        if (Animation == null) return;
        Animation.AnimationState.SetAnimation(0, "action2", false).Complete += (trackEntry) =>
        {
            Animation.AnimationState.SetAnimation(0, "action3", true);
        };
    }
    public void OnScrewsCountChanged(int count)
    {
        if (count <= 0)
        {
            OnRemoveAllScrews();
        }
    }
    [Button]
    public void RemoveScrew(Screw screw)
    {
        if (Screws.Contains(screw))
        {
            Screws.Remove(screw);
            screw.OnRemoveScrew();
            DecreaseScrewCount();
            PlayRemoveScrewAnimation();
            
        }
    }
    public bool TryRemoveScrew(out ColorId colorId)
    {
        if (Screws.Count > 0)
        {
            Screw screw = Screws[0];
            colorId = screw.ColorId;
            RemoveScrew(screw);
            return true;
        }

        colorId = ColorId.None; // Assuming ColorId.None is a valid default value
        return false;
    }
    public void DecreaseScrewCount()
    {
        if (CurrentScrewCount.Value > 0)
        {
            CurrentScrewCount.Value--;
        }
    }
    private void  OnRemoveAllScrews()
    {
        //this.gameObject.SetActive(false);
        //await UniTask.WaitForSeconds(1.3f);
        PlayRemoveAllScrewsAnimation();
    }
    public Vector3 GetCurrentScrewPosition()
    {
        if (Screws.Count > 0)
        {
            return Screws[0].transform.position;
        }
        return Vector3.zero; // or some default position
    }
    public int GetNumScrewsColor(ColorId colorId)
    {
        int count = 0;
        for (var i = 0; i < Screws.Count; i++)
        {
            if(Screws[i].ColorId == colorId)
                count++;
        }
        return count;
    }
#if UNITY_EDITOR
    public void SetId(int id)
    {
        this.Id = id;
    }
    public void GetAllScrews()
    {
        this.Screws.Clear();
        this.Screws.AddRange(this.GetComponentsInChildren<Screw>());
        for (var i = 0; i < Screws.Count; i++)
        {
            Screws[i].Init(this);
        }
    }
    [Button]
    public void PlayAnimation(int index)
    {
        if (Animation == null) return;
        string firstAction = "action" + (index);
        string secondAction = "action" + (index + 1);
        Animation.AnimationState.SetAnimation(index, firstAction, false).Complete += (trackEntry) =>
        {
            Animation.AnimationState.SetAnimation(index, secondAction, true);
        };
    }
#endif
}
