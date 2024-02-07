namespace UralHedgehog
{
    public interface ISettings
    {
        float VolumeMaster { get; }
        float VolumeMusic { get; }
        float VolumeSound { get; }
        float VolumeVoice { get; }
        Language Language { get; }

        void ChangeVolumeMaster(float value);
        void ChangeVolumeMusic(float value);
        void ChangeVolumeSound(float value);
        void ChangeVolumeVoice(float value);
        void ChangeLanguage(Language language);
    }
}