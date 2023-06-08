using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Inscription_mp
{
    public class SoundEngine
    {
        MediaPlayer mp = new MediaPlayer() { Volume = 80 };
        Random random = new Random();

        Dictionary<Songs, string> songs = new Dictionary<Songs, string>()
        {
            { Songs.MainMusic, @"./Resources/Audio/music/gametable_ambience.wav" }
        };
        public enum Songs
        {
            MainMusic,

        };

        Dictionary<Sfx, string[]> sfx = new Dictionary<Sfx, string[]>()
        {
            { Sfx.Click, new string []{
                @"./Resources/Audio/sfx/command_line_click#1.wav",
                @"./Resources/Audio/sfx/command_line_click#2.wav",
                @"./Resources/Audio/sfx/command_line_click#3.wav",
                @"./Resources/Audio/sfx/command_line_click#4.wav",
                @"./Resources/Audio/sfx/command_line_click#5.wav" }
            },
            { Sfx.Card, new string[]{
                    @"./Resources/Audio/sfx/card#1.wav",
                    @"./Resources/Audio/sfx/card#2.wav",
                    @"./Resources/Audio/sfx/card#3.wav",
                    @"./Resources/Audio/sfx/card#4.wav",
                    @"./Resources/Audio/sfx/card#5.wav",
                    @"./Resources/Audio/sfx/card#6.wav",
                    @"./Resources/Audio/sfx/card#7.wav",
                    @"./Resources/Audio/sfx/card#8.wav",
                    @"./Resources/Audio/sfx/card#9.wav",
                    @"./Resources/Audio/sfx/card#10.wav"}
            },
            { Sfx.Card_attack_creature, new string[]{
                @"./Resources/Audio/sfx/card_attack_creature.wav"}
            },
            { Sfx.Card_attack_directly, new string[]{
                @"./Resources/Audio/sfx/card_attack_directly.wav"}
            },
            { Sfx.Card_attack_damage, new string[]{
                @"./Resources/Audio/sfx/card_attack_damage.wav"}
            },
            { Sfx.Card_blessing, new string[]{
                @"./Resources/Audio/sfx/card_blessing.wav"}
            },
            { Sfx.Card_death, new string[]{
                @"./Resources/Audio/sfx/card_death.wav"}
            },
            { Sfx.Cardslot_glow, new string[]{
                @"./Resources/Audio/sfx/Cardslot_glow#1.wav",
                @"./Resources/Audio/sfx/Cardslot_glow#2.wav",
                @"./Resources/Audio/sfx/Cardslot_glow#3.wav",
                @"./Resources/Audio/sfx/Cardslot_glow#4.wav", }
            },
            { Sfx.Crunch_blip, new string[]{
                @"./Resources/Audio/sfx/Crunch_blip.wav"}
            },
            { Sfx.Crunch_short, new string[]{
                @"./Resources/Audio/sfx/Crunch_short#1.wav",
                @"./Resources/Audio/sfx/Crunch_short#2.wav"}
            },
            { Sfx.Map_move, new string[]{
                @"./Resources/Audio/sfx/Map_move#1.wav",
                @"./Resources/Audio/sfx/Map_move#2.wav",
                @"./Resources/Audio/sfx/Map_move#3.wav"}
            }
        };
        public enum Sfx
        {
            Click,
            Card,
            Card_attack_creature,
            Card_attack_directly,
            Card_attack_damage,
            Card_blessing,
            Card_death,
            Cardslot_glow,
            Crunch_blip,
            Crunch_short,
            Map_move,
        };

        public void PlaySound(Sfx sound, bool repeat = false)
        {

            string[] dicOut;
            sfx.TryGetValue(sound, out dicOut);
            MediaPlayer mp = new MediaPlayer() { Volume = 80 };
            int indexDicOut = 0;
            if (dicOut.Length > 1)
            {
                indexDicOut = random.Next(dicOut.Length);
            }
            mp.Open(new Uri(dicOut[indexDicOut], UriKind.Relative));
            mp.Play();
        }
        public void StopSound(/*unknown*/)
        {
            mp.Stop();
        }
        public void PlaySong(Songs song)
        {
            string dicOut;
            songs.TryGetValue(Songs.MainMusic, out dicOut);
            mp.MediaEnded += (s, e) => { mp.Position = TimeSpan.MinValue; mp.Play(); };
            mp.Open(new Uri(dicOut, UriKind.Relative));
            mp.Play();
        }

        public void PlaySong(string song)
        {

        }
    }
}
