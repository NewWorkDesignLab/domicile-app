using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRControls : MonoBehaviour
{
  public static VRControls instance;
  public Rigidbody rb;
  public Image walkCircle;
  public GameObject head;
  public GameObject menu;

  public float speed = .2f;

  public float headNormalHeight = 1.5f;
  public float headCrawlHeight = 0f;
  public float menuNormalScale = .005f;
  public float menuCrawlScale = .0025f;
  PlayerStatus playerStatus;

  private Coroutine onWalkCloseMenu;

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    SetModeIdle();
  }

  void Update()
  {
    if (playerStatus == PlayerStatus.walk)
    {
      MovePlayer();
      if (onWalkCloseMenu == null && IngameMenuScript.instance.mainMenu.gameObject.active) {
        onWalkCloseMenu = StartCoroutine(CloseMenuAfterConstantWalk());
      }
    }
    if (Input.touchCount > 0)
    {
      IngameMenuScript.instance.mainMenu.SetActive(false);
      IngameMenuScript.instance.menuButton.SetActive(false);
      ScreenshotScript.instance.TakeScreenshot(() => {
        IngameMenuScript.instance.menuButton.SetActive(true);
      });
    }
  }

  public void MovePlayer()
  {
    Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
    Vector3 fixedForward = new Vector3((float)forward.x, 0f, (float)forward.z).normalized;
    Vector3 movement = fixedForward * speed * Time.deltaTime;
    rb.MovePosition(transform.position + movement);
  }

  public void SetModeWalk()
  {
    playerStatus = PlayerStatus.walk;
  }

  public void SetModeIdle()
  {
    playerStatus = PlayerStatus.idle;
    walkCircle.enabled = true;
    SetHeadPosition(headNormalHeight);
    SetMenuScale(menuNormalScale);
    HUD.instance.crouchIcon.SetActive(false);
    HUD.instance.standIcon.SetActive(true);
  }

  public void SetModeCrawl()
  {
    playerStatus = PlayerStatus.crawl;
    walkCircle.enabled = false;
    SetHeadPosition(headCrawlHeight);
    SetMenuScale(menuCrawlScale);
    HUD.instance.standIcon.SetActive(false);
    HUD.instance.crouchIcon.SetActive(true);
  }

  private void SetHeadPosition(float value)
  {
    Vector3 newPos = head.gameObject.transform.localPosition;
    newPos.y = value;
    head.gameObject.transform.localPosition = newPos;
  }
  private void SetMenuScale(float value)
  {
    Vector3 newScale = new Vector3(value, value, value);
    menu.gameObject.transform.localScale = newScale;
  }

  public void ToggleModeCrawl()
  {
    if (playerStatus == PlayerStatus.idle)
    {
      SetModeCrawl();
    }
    else
    {
      SetModeIdle();
    }
  }

  private IEnumerator CloseMenuAfterConstantWalk() {
    yield return new WaitForSeconds(1f);
    if (playerStatus == PlayerStatus.walk) {
      IngameMenuScript.instance.mainMenu.SetActive(false);
      IngameMenuScript.instance.menuButton.SetActive(true);
    }
    onWalkCloseMenu = null;
  }
}

public enum PlayerStatus { idle, walk, crawl };
