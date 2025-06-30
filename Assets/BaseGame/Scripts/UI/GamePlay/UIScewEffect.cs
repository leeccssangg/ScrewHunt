using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DG.Tweening;
using TW.Utility.CustomComponent;
using UnityEngine;
using UnityEngine.UI;

public class UIScewEffect : ACachedMonoBehaviour
{
    [field: SerializeField] public UIScrewBox UIScrewBox { get; private set; }
    private List<Tween> TweenList { get; set; } = new List<Tween>();
    [field: SerializeField] public Image ImgScrewIcon { get; private set; }
    [field: SerializeField] public ParticleImage UITrail { get; private set; }
    public void Setup(Vector3 startPos, Vector3 endPos, UIScrewBox uiScrewBox)
    {
        AudioManager.Instance.PlaySoundFx(AudioType.SfxPickupBlock);
        UIScrewBox = uiScrewBox;
        Transform.localScale = Vector3.one;
        ImgScrewIcon.sprite = SpritesGlobalConfig.Instance.GetScrewEffectSprite(UIScrewBox.ScrewBox.ColorId);
        //ColorUtil.SetColorToImage(this.ImgScrewIcon, UIScrewBox.ScrewBox.ColorId);
        //Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-250f, 250f), UnityEngine.Random.Range(-250f, 250f), 0);
        // float randomDelay = UnityEngine.Random.Range(0f, 0.5f);
        // TweenList.Add(Transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f));
        // TweenList.Add(Transform.DOMove(startPos + randomPos , 0.5f).SetEase(Ease.InOutSine).SetDelay(0.5f));
        // TweenList.Add(Transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine).SetDelay(0.5f));
        Vector3 randomPos = new Vector3(-250f, 250f, 0);
        Tween tween =
            Transform.DOMove(startPos + randomPos, 0.5f).SetEase(Ease.InOutSine)
                .OnStart(() =>
                {
                    UITrail.Stop();
                })
                .OnComplete(() =>
                {
                    Transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine).SetDelay(0.65f)
                        .OnStart(() =>
                        {
                            UITrail.gameObject.SetActive(true);
                            UITrail.Play();
                        })
                        .OnComplete(() =>
                    {
                        ScreenInGameContext.Events.DespawnScrew?.Invoke(this, this.UIScrewBox);
                    });
                });
        TweenList.Add(tween);
        // Create a sequence to blend animations
        // Sequence sequence = DOTween.Sequence();
        //
        // // Stretch animation while moving to randomPos
        // sequence.Append(Transform.DOMove(startPos + randomPos, 0.5f).SetEase(Ease.InOutSine))
        //     .Append(Transform.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.3f).SetEase(Ease.OutElastic))
        //     .Append(Transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic));
        //
        // // Move to endPos after reaching randomPos
        // sequence.Append(Transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine))
        //     .OnComplete(() =>
        //     {
        //         ScreenInGameContext.Events.DespawnScrew?.Invoke(this, this.UIScrewBox);
        //     });
        //
        // // Add sequence to TweenList for cleanup
        // TweenList.Add(sequence);
    }

    private void OnDestroy()
    {
        TweenList.ForEach(tween => tween.Kill());
        TweenList.Clear();
    }
    public void DeactiveTrail()
    {
        if (UITrail != null)
        {
            UITrail.gameObject.SetActive(false);
            UITrail.Stop();
        }
    }
}

