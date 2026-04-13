using UnityEngine;

public class ThemesAndSounds : MonoBehaviour
{
    public static ThemesAndSounds Instance;

    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip winSound;
    public AudioClip hitSound;
    public AudioClip stepSound;
    public AudioClip bgMusic;

    private AudioSource sfxSource;
    private AudioSource bgmSource;
    private AudioSource stepSource;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.volume = 0.5f;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.4f;
    }

    private void Start()
    {
        ApplyAdventureTheme();
        GenerateSounds();
        bgmSource.clip = bgMusic;
        bgmSource.Play();
    }

    private void ApplyAdventureTheme()
    {
        // Dark twilight/midnight adventure sky
        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(0.05f, 0.08f, 0.12f);
        }

        // Color walls and floors to adventure dungeon/forest theme
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material[] mats = r.materials;
            for(int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains("Wall"))
                {
                    // Dark Wood/Dirt Brown
                    mats[i].color = new Color(0.35f, 0.2f, 0.1f);
                }
                else if (mats[i].name.Contains("Grid") || mats[i].name.Contains("Floor"))
                {
                    // Mossy Forest Green
                    mats[i].color = new Color(0.15f, 0.35f, 0.15f);
                }
            }
        }
    }

    private void GenerateSounds()
    {
        jumpSound = CreateTone(300f, 0.3f, 1);
        coinSound = CreateTone(880f, 0.1f, 2);
        winSound = CreateTone(400f, 2.0f, 3);
        hitSound = CreateTone(100f, 0.5f, 5); 
        stepSound = CreateTone(50f, 0.15f, 6); // Footstep crunch
        bgMusic = CreateTone(110f, 4.0f, 4); // Brooding adventure drum/drone
    }

    private AudioClip CreateTone(float frequency, float duration, int type)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        float[] sampleData = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            switch(type)
            {
                case 1: // Jump (Swoosh up)
                    sampleData[i] = Mathf.Sin(2 * Mathf.PI * (frequency + (t * 600f)) * t) * (1f - (t / duration));
                    break;
                case 2: // Coin (High ping)
                    sampleData[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * Mathf.Exp(-t * 20f);
                    break;
                case 3: // Win (Chord)
                    float freq = frequency * (1f + (int)(t * 4f) * 0.25f);
                    sampleData[i] = Mathf.Sin(2 * Mathf.PI * freq * t) * (1f - (t / duration));
                    break;
                case 4: // BGM (Low brooding heartbeat)
                    float beatEnv = 1.0f - ((t * 2f) % 1.0f); 
                    sampleData[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * beatEnv * 0.4f;
                    sampleData[i] += Mathf.Sin(2 * Mathf.PI * (frequency * 0.5f) * t) * 0.3f; 
                    break;
                case 5: // Hit (Crunch)
                    float noise = Random.Range(-1f, 1f);
                    sampleData[i] = (Mathf.Sin(2 * Mathf.PI * frequency * t) * 0.5f + noise * 0.5f) * Mathf.Exp(-t * 10f);
                    break;
                case 6: // Footstep (Muted crunch)
                    float stepNoise = Random.Range(-1f, 1f);
                    sampleData[i] = stepNoise * Mathf.Exp(-t * 30f) * 0.5f;
                    break;
            }
        }

        AudioClip clip = AudioClip.Create("SynthesizedTone", sampleCount, 1, sampleRate, false);
        clip.SetData(sampleData, 0);
        return clip;
    }

    public void PlayJump() { if(sfxSource) sfxSource.PlayOneShot(jumpSound); }
    public void PlayCoin() { if(sfxSource) sfxSource.PlayOneShot(coinSound); }
    public void PlayWin() { if(sfxSource) sfxSource.PlayOneShot(winSound); }
    public void PlayHit() { if(sfxSource) sfxSource.PlayOneShot(hitSound); }
    public void PlayStep() { if(stepSource) stepSource.PlayOneShot(stepSound); }
}
