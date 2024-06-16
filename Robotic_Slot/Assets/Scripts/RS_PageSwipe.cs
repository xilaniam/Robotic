using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RS_PageSwipe : MonoBehaviour
{

    [SerializeField] private GameObject _pages;
    [SerializeField] private UnityEngine.UI.Image[] barImages;
    [SerializeField] private Sprite active, inactive;
    [SerializeField] float image_width = 14;

    int _currentPage = 0;
    int maxPage;

    private void Awake()
    {

    }
    void Start()
    {
        maxPage = _pages.transform.childCount-1;

        UpdateImageBar();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwipePageRight()
    {
       // audioManager.playSFX(audioManager.buttonSound);
        if (_currentPage < maxPage) 
        {
            _pages.transform.localPosition = new Vector3(_pages.transform.localPosition.x - image_width, _pages.transform.localPosition.y, _pages.transform.localPosition.z);
            _currentPage++;
        }
        else
        {
            _pages.transform.localPosition = new Vector3(_pages.transform.localPosition.x + (image_width*maxPage), _pages.transform.localPosition.y, _pages.transform.localPosition.z);
            _currentPage = 0;
        }
        UpdateImageBar();
    }
    public void SwipePageLeftt()
    {
       // audioManager.playSFX(audioManager.buttonSound);
        if (_currentPage > 0)
        {
            _pages.transform.localPosition = new Vector3(_pages.transform.localPosition.x + image_width, _pages.transform.localPosition.y, _pages.transform.localPosition.z);
            _currentPage--;
        }
        else
        {
            _pages.transform.localPosition = new Vector3(_pages.transform.localPosition.x - (image_width * maxPage), _pages.transform.localPosition.y, _pages.transform.localPosition.z);
            _currentPage = maxPage;
        }
        UpdateImageBar();
    }

    void UpdateImageBar()
    {
        foreach (var image in barImages) 
        {
            image.sprite = inactive;
        }
        barImages[_currentPage].sprite = active;
    }
}
