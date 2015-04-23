using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
    // de states
    private AudioSource m_SoundSource;
    private AudioSource m_MusicSource;

    // de properties
    public float SoundVolume
    {
        get { return m_SoundSource.volume; }
        set { m_SoundSource.volume = value; }
    }
    public float MusicVolume
    {
        get { return m_MusicSource.volume; }
        set { m_MusicSource.volume = value; }
    }

    public int HighScore { get; set; }
    public void Load(AudioSource Music, AudioSource Sound)
    {
        m_SoundSource = Sound;
        m_MusicSource = Music;
        // Ik had 1,0 in plaats van 1.0 dus ik was bezig de parameter aan het overloaden. Niet handig.
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void Save() 

        // Ik gebruikte setfloat inplaats van getfloat. Daardoor werkte mijn script niet.
    {
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("HighScore", HighScore);
    }
}
