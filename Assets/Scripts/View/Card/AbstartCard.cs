using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Card
{
	[RequireComponent(typeof(Image))]
	public abstract class AbstractCardView : MonoBehaviour, IPointerDownHandler
	{
		public event Action<AbstractCardView> OnClick;
		[SerializeField] private Sprite[] _sides = new Sprite[2];
		private Image _image;
		private CardModel _cardModel;
		public int Id => _cardModel.GetId;

		public void Setup(CardModel cardModel, Sprite sprite)
		{
			_cardModel = cardModel;
			_image = GetComponent<Image>();
			_image.sprite = sprite;
			_sides[1] = sprite;
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			OnClick?.Invoke(this);
		}

		public virtual void Flip(bool up, float flipTime, Action onComplete = null)
		{
			Flip(up);
			DOVirtual.DelayedCall(flipTime, (() =>
			{
				Flip(!up);
				onComplete?.Invoke();
			}));
		}

		public virtual void Flip(bool up)
		{
			_image.sprite = up ? _sides[1] : _sides[0];
		}
	}
}
