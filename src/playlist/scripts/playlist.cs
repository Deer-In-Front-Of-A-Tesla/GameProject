using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

/// <summary>
///     Universal access point for all music playing, beats, strengths etc. Written by Red
/// </summary>
public class playlist : Node
{
	[Signal]
	public delegate void GameBeat(float strength);

	[Export] public bool _auto_song_load;

	[Export] public NodePath _player;

	[Export] public Resource[] _songs;

	public static GameSong currentlyPlaying;

	private readonly Dictionary<string, GameSong> GameSongs = new Dictionary<string, GameSong>();
	private Beat lastBeat;
	private AudioStreamPlayer player;
	private bool randomContinuous;

	/// <summary>
	///     Reloads songs from the Godot resource. If you want to add songs, add a song there then call this method.
	///     If autoLoad is true, it also tries to load all resources in res://src/playlist/assets/ as songs (not recursive)
	/// </summary>
	public void ReloadSongs(bool autoLoad)
	{
		GameSongs.Clear();
		if (autoLoad)
		{
			var dir = new Directory();
			var path = "res://src/playlist/assets/";
			dir.Open(path);
			dir.ListDirBegin();
			string filename;
			do
			{
				filename = dir.GetNext();
				if (filename.EndsWith(".tres"))
					try
					{
						var newSong = new GameSong(ResourceLoader.Load(path + filename), this);
						GameSongs.Add(newSong.song_id, newSong);
					}
					catch (Exception e)
					{
						GD.PrintErr("Song resource invalid, autoload skipping.\nSong: " + path + filename +
									"\nReason: " + e.Message);
					}
			} while (filename != "");
		}

		foreach (var _song in _songs)
		{
			var newSong = new GameSong(_song, this);
			GameSongs[newSong.song_id] = newSong;
		}
	}

	/// <summary>
	///     Starts playing the song specified by id, return the GameSong object
	/// </summary>
	public GameSong PlaySong(string id)
	{
		var toPlay = GameSongs[id];
		return _PlaySong(toPlay);
	}

	public GameSong GetCurrentSong()
	{
		return currentlyPlaying;
	}

	public GameSong PlayRandom()
	{
		randomContinuous = true;
		var rand = new Random();
		var indNext = rand.Next(GameSongs.Count);
		GameSong nextSong = GameSongs.Values.ToList()[indNext];
		return _PlaySong(nextSong);
	}

	private GameSong _PlaySong(GameSong song)
	{
		currentlyPlaying = song;
		song.Play();
		return song;
	}

	public override void _Ready()
	{
		ReloadSongs(_auto_song_load);
		player = GetNode<AudioStreamPlayer>(_player);
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		if (!player.Playing && player.Stream != null)
		{
			// emit signal indicating that playing a song just stopped
			player.Stream = null;
			player.Playing = false;
			if (randomContinuous)
			{
				PlayRandom();
			}
		}

		if (currentlyPlaying != null && currentlyPlaying.GetLastBeat() != lastBeat)
		{
			lastBeat = currentlyPlaying.GetLastBeat();
			EmitSignal(nameof(GameBeat), lastBeat.strength);
		}
	}

	public class Beat
	{
		private readonly float offsetCoefficient;
		private readonly GameSong song;

		public Beat(float offset, float strength, GameSong song)
		{
			offsetCoefficient = offset;
			this.strength = strength;
			this.song = song;
		}

		public float offset => offsetCoefficient * song.tact_length;
		public float strength { get; } = 1;

		public float CalculateStrength(float offset)
		{
			if (this.offset.Equals(offset)) return strength;
			if (offset < this.offset) return 0;

			var strictness = (float)ProjectSettings.GetSetting("music/beat_strictness");
			var steepness = (float)ProjectSettings.GetSetting("music/steepness");
			var offsetDelta = Math.Abs(offset - this.offset);

			var score = Math.Abs(strictness / offsetDelta) - steepness;
			if (score <= 0) return 0;
			if (score >= 1) return strength;

			return score * strength;
		}
	}

	/// <summary>
	///     This class represents a song, along with its beats, offset and BPM. Should be loaded from a resource file.
	/// </summary>
	public class GameSong
	{
		private readonly List<Beat> beats = new List<Beat>();
		private readonly int beats_per_tact;
		private readonly float bpm;
		private readonly float offset;

		private readonly playlist parentPlaylist;
		private readonly AudioStreamMP3 source;


		/// <summary>
		///     Creates a C# GameSong object based on a Resource. Ensure this is synced with the data/game-music-data.gd script
		/// </summary>
		public GameSong(Resource _source, playlist playlist)
		{
			name = (string)_source.Get("name");
			song_id = (string)_source.Get("song_id");
			bpm = (float)_source.Get("bpm");
			offset = (float)_source.Get("offset");
			beats_per_tact = (int)_source.Get("beats_per_tact");
			source = (AudioStreamMP3)_source.Get("source");
			tact_length = 60 / bpm * beats_per_tact;
			parentPlaylist = playlist;

			var strengths = _source.Get("beat_strengths");
			var beats = (Vector2[])strengths;

			foreach (var beat in beats) this.beats.Add(new Beat(beat.x, beat.y, this));
			if (this.beats.Count == 0)
			{
				this.beats.Add(new Beat(0, 1, this));
			}
		}

		public string name { get; } = "DefaultSongName";
		public string song_id { get; } = "DefaultSongID";
		public float tact_length { get; } = 1f;

		/// <summary>
		///     Starts playing this song in the specified player
		/// </summary>
		public void Play()
		{
			GD.Print("Playing song");
			var player = parentPlaylist.player;
			player.Stream = source;
			player.Autoplay = true;

			player.Play(offset);
		}

		public Beat GetLastBeat()
		{
			var currentOffset = GetCurrentSongOffset();
			for (var i = beats.Count - 1; i > 0; i--)
				if (beats[i].offset <= currentOffset)
					return beats[i];
			return beats[0];
		}


		/// <summary>
		///     Returns the current strength of the beat, based on how close it is to one. Can return ApplicationException
		///     in case this song is not being played at the moment.
		/// </summary>
		public float GetCurrentStrength()
		{
			if (parentPlaylist == null || parentPlaylist.player.Stream != source)
				throw new ApplicationException("Can't get beat strength");
			float strength = 0;
			var currentOffset = GetCurrentSongOffset();

			foreach (var beat in beats) strength = Math.Max(beat.CalculateStrength(currentOffset), strength);

			return strength;
		}

		public float GetCurrentSongOffset()
		{
			var personalOffset = (float)ProjectSettings.GetSetting("music/personal_offset");
			return (parentPlaylist.player.GetPlaybackPosition() - offset - personalOffset) % tact_length;
		}

		private float GetOffsetDiff(float offset1, float offset2)
		{
			return Math.Min(Math.Abs(offset1 - offset2), Math.Abs(offset1 - offset2 + tact_length));
		}

		public void AdjustPersonalOffset()
		{
			var currentOffset = GetCurrentSongOffset();
			var bestOffset = beats[0].CalculateStrength(currentOffset);
			var bestOffsetInd = 0;
			for (var i = 0; i < beats.Count; i++)
				if (beats[i].CalculateStrength(currentOffset) < bestOffset)
				{
					bestOffset = beats[i].CalculateStrength(currentOffset);
					bestOffsetInd = i;
				}

			var targetBeat = beats[bestOffsetInd];
			if (targetBeat.CalculateStrength(currentOffset) > targetBeat.strength * 0.5) return;

			var strictness = (float)ProjectSettings.GetSetting("music/beat_strictness");
			var steepness = (float)ProjectSettings.GetSetting("music/steepness");
			var goalOffset = targetBeat.offset + strictness / ((steepness + 1f) * 2);
			var currentPersonalOffset = (float)ProjectSettings.GetSetting("music/personal_offset");
			var newOffset = currentPersonalOffset + (goalOffset - currentOffset) * 0.01f;

			GD.Print("Current offset: " + currentOffset +
					 "\nCurrent strength: " + targetBeat.CalculateStrength(currentOffset) +
					 "\nTarget beat offset: " + targetBeat.offset +
					 "\nGoalOffset:; " + goalOffset +
					 "\nNew personal offset: " + newOffset +
					 "\n"
			);

			ProjectSettings.SetSetting("music/personal_offset", newOffset);
		}
	}
}
