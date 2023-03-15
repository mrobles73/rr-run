using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  private static AudioManager _instance;
  [Header("Ambient Audio")]
  public AudioClip menuClip;
  public AudioClip[] musicClips;
  public AudioClip roofDrillClip;
  public AudioClip spikesUpClip;
  public AudioClip crusherDownClip;
  public AudioClip bubblesClip;
  [Header("Stings")]
  public AudioClip[] shotStingClips;
  public AudioClip gemsCollectStingClip;
  public AudioClip levelFinishStingClip;
  public AudioClip winStingClip;
  public AudioClip trapButtonStingClip;
  [Header("Explosions")]
  public AudioClip playerExplosionClip;
  public AudioClip enemyExplosionClip;
  public AudioClip bombExplosionClip;
  public AudioClip foodExplosionClip;
  public AudioClip barrelKilledClip;
  public AudioClip barrelHitStingClip;
  [Header("RedBot Audio")]
  public AudioClip walkStepClip;
  public AudioClip crouchStepClip;
  public AudioClip hitClip;
  public AudioClip deathClip;
  public AudioClip jumpClip;
  public AudioClip winVoiceClip;
  public AudioClip gemVoiceClip;
  [Header("Enemy Audio")]
  public AudioClip[] enemyWalkStepClips;
  public AudioClip enemyHitClip;
  public AudioClip enemyDeathClip;
  [Header("Mixer Groups")]
  public AudioMixerGroup ambientGroup;
  public AudioMixerGroup musicGroup;
  public AudioMixerGroup stingGroup;
  public AudioMixerGroup explosionGroup;
  public AudioMixerGroup enemyGroup;
  public AudioMixerGroup playerGroup;
  public AudioMixerGroup voiceGroup;
  private AudioSource _ambientSource;
  private AudioSource _musicSource;
  private AudioSource _stingSource;
  private AudioSource _explosionSource;
  private AudioSource _enemySource;
  private AudioSource _playerSource;
  private AudioSource _voiceSource;

  private void Awake()
  {
    if ((Object) AudioManager._instance == (Object) null)
    {
      AudioManager._instance = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
      this._ambientSource = this.gameObject.AddComponent<AudioSource>();
      this._musicSource = this.gameObject.AddComponent<AudioSource>();
      this._stingSource = this.gameObject.AddComponent<AudioSource>();
      this._explosionSource = this.gameObject.AddComponent<AudioSource>();
      this._enemySource = this.gameObject.AddComponent<AudioSource>();
      this._playerSource = this.gameObject.AddComponent<AudioSource>();
      this._voiceSource = this.gameObject.AddComponent<AudioSource>();
      this._ambientSource.outputAudioMixerGroup = this.ambientGroup;
      this._musicSource.outputAudioMixerGroup = this.musicGroup;
      this._stingSource.outputAudioMixerGroup = this.stingGroup;
      this._explosionSource.outputAudioMixerGroup = this.explosionGroup;
      this._enemySource.outputAudioMixerGroup = this.enemyGroup;
      this._playerSource.outputAudioMixerGroup = this.playerGroup;
      this._playerSource.outputAudioMixerGroup = this.voiceGroup;
      AudioManager.StartMenuAudio();
    }
    else
      Object.Destroy((Object) this.gameObject);
  }

  public static void StartMenuAudio()
  {
    AudioManager._instance._musicSource.clip = AudioManager._instance.menuClip;
    AudioManager._instance._musicSource.loop = true;
    AudioManager._instance._musicSource.Play();
  }

  public static void callLevelAudio() => AudioManager._instance.StartCoroutine(AudioManager.StartLevelAudio());

  private static IEnumerator StartLevelAudio()
  {
    yield return (object) null;
    int i = 0;
    for (i = 0; i < AudioManager._instance.musicClips.Length; ++i)
    {
      AudioManager._instance._musicSource.clip = AudioManager._instance.musicClips[i];
      AudioManager._instance._musicSource.Play();
      Debug.Log((object) "StartedLevelAudio");
      while (AudioManager._instance._musicSource.isPlaying)
        yield return (object) null;
    }
    if (i >= AudioManager._instance.musicClips.Length)
      AudioManager.callLevelAudio();
  }

  public static void PlayFootstepAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._playerSource.isPlaying)
      AudioManager._instance._playerSource.Stop();
    AudioManager._instance._playerSource.clip = AudioManager._instance.walkStepClip;
    AudioManager._instance._playerSource.Play();
  }

  public static void PlayEnemyFootstepAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._enemySource.isPlaying)
      AudioManager._instance._enemySource.Stop();
    int index = Random.Range(0, AudioManager._instance.enemyWalkStepClips.Length);
    AudioManager._instance._enemySource.clip = AudioManager._instance.enemyWalkStepClips[index];
    AudioManager._instance._enemySource.Play();
  }

  public static void PlayCrouchFootstepAudio()
  {
    if ((Object) AudioManager._instance == (Object) null || AudioManager._instance._playerSource.isPlaying)
      return;
    AudioManager._instance._playerSource.clip = AudioManager._instance.crouchStepClip;
    AudioManager._instance._playerSource.Play();
  }

  public static void PlayJumpAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._playerSource.clip = AudioManager._instance.jumpClip;
    AudioManager._instance._playerSource.Play();
  }

  public static void PlayHitAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._playerSource.isPlaying)
      AudioManager._instance._playerSource.Stop();
    AudioManager._instance._playerSource.clip = AudioManager._instance.hitClip;
    AudioManager._instance._playerSource.Play();
  }

  public static void PlayEnemyHitAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._enemySource.isPlaying)
      AudioManager._instance._enemySource.Stop();
    AudioManager._instance._enemySource.clip = AudioManager._instance.enemyHitClip;
    AudioManager._instance._enemySource.Play();
  }

  public static void PlayDeathAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._playerSource.isPlaying)
      AudioManager._instance._playerSource.Stop();
    AudioManager._instance._playerSource.clip = AudioManager._instance.deathClip;
    AudioManager._instance._playerSource.Play();
  }

  public static void PlayEnemyDeathAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._enemySource.isPlaying)
      AudioManager._instance._enemySource.Stop();
    AudioManager._instance._enemySource.clip = AudioManager._instance.enemyDeathClip;
    AudioManager._instance._enemySource.Play();
  }

  public static void PlayLevelFinishAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._voiceSource.clip = AudioManager._instance.winVoiceClip;
    AudioManager._instance._voiceSource.Play();
    AudioManager._instance._stingSource.clip = AudioManager._instance.levelFinishStingClip;
    AudioManager._instance._stingSource.Play();
  }

  public static void PlayWinAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._ambientSource.Stop();
    AudioManager._instance._stingSource.clip = AudioManager._instance.winStingClip;
    AudioManager._instance._stingSource.Play();
  }

  public static void PlayGemCollectAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._stingSource.isPlaying)
      AudioManager._instance._stingSource.Stop();
    AudioManager._instance._stingSource.clip = AudioManager._instance.gemsCollectStingClip;
    AudioManager._instance._stingSource.Play();
    AudioManager._instance._voiceSource.clip = AudioManager._instance.gemVoiceClip;
    AudioManager._instance._voiceSource.Play();
  }

  public static void PlayShotAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    int index = Random.Range(0, AudioManager._instance.shotStingClips.Length);
    AudioManager._instance._stingSource.clip = AudioManager._instance.shotStingClips[index];
    AudioManager._instance._stingSource.Play();
  }

  public static void PlayTrapButtonAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._stingSource.clip = AudioManager._instance.trapButtonStingClip;
    AudioManager._instance._stingSource.Play();
  }

  public static void PlayPlayerExplosionAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._explosionSource.clip = AudioManager._instance.playerExplosionClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayEnemyExplosionAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._explosionSource.clip = AudioManager._instance.enemyExplosionClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayBombExplosionAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._explosionSource.clip = AudioManager._instance.bombExplosionClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayFoodExplosionAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._explosionSource.clip = AudioManager._instance.foodExplosionClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayBarrelDeathAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._explosionSource.isPlaying)
      AudioManager._instance._explosionSource.Stop();
    AudioManager._instance._explosionSource.clip = AudioManager._instance.barrelKilledClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayBarrelHitAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    if (AudioManager._instance._explosionSource.isPlaying)
      AudioManager._instance._explosionSource.Stop();
    AudioManager._instance._explosionSource.clip = AudioManager._instance.barrelHitStingClip;
    AudioManager._instance._explosionSource.Play();
  }

  public static void PlayRoofDrillAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._ambientSource.clip = AudioManager._instance.roofDrillClip;
    AudioManager._instance._ambientSource.Play();
  }

  public static void PlaySpikeAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._ambientSource.clip = AudioManager._instance.spikesUpClip;
    AudioManager._instance._ambientSource.Play();
  }

  public static void PlayCrusherAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._ambientSource.clip = AudioManager._instance.crusherDownClip;
    AudioManager._instance._ambientSource.Play();
  }

  public static void PlayBubbleAudio()
  {
    if ((Object) AudioManager._instance == (Object) null)
      return;
    AudioManager._instance._ambientSource.clip = AudioManager._instance.bubblesClip;
    AudioManager._instance._ambientSource.loop = true;
    AudioManager._instance._ambientSource.Play();
  }
}
