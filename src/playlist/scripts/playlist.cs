using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Universal access point for all music playing, beats, strengths etc. Written by Red
/// </summary>
public class playlist : Node
{
	[Export]
	public Resource[] _songs;

	[Export] 
	public NodePath _player;

	[Export] 
	public bool _auto_song_load;

	private Dictionary<string, GameSong> GameSongs = new Dictionary<string, GameSong>();
	private AudioStreamPlayer player;
	
	class Beat
	{
		private float offset;
		private float strength;

		public Beat(float offset, float strength)
		{
			this.offset = offset;
			this.strength = strength;
		}

		public float calculateStrength(float offset)
		{
			if (this.offset.Equals(offset)) return strength;
			if (offset < this.offset) return 0;

			float strictness = (float) ProjectSettings.GetSetting("music/beat_strictness");
			float steepness = (float) ProjectSettings.GetSetting("music/steepness");
			float offset_delta = Math.Abs(offset - this.offset);

			float score = Math.Abs(strictness / offset_delta) - steepness;
			if (score <= 0) return 0;
			if (score >= 1) return strength;

			return score * strength;
		}
	}
	
	/// <summary>
	/// This class represents a song, along with its beats, offset and BPM. Should be loaded from a resource file.
	/// </summary>
	public class GameSong
	{
		public string name { get; } = "DefaultSongName";
		public string song_id{ get; } = "DefaultSongID";
		private float offset;
		private float bpm;
		private int beats_per_tact;
		private AudioStreamMP3 source;
		private List<Beat> beat_strengths = new List<Beat>();
		private float tact_length;

		private AudioStreamPlayer lastPlayer;
		
		
		/// <summary>
		/// Creates a C# GameSong object based on a Resource. Ensure this is synced with the data/game-music-data.gd script
		/// </summary>
		public GameSong(Resource _source)
		{
			name = (string)_source.Get("name");
			song_id = (string)_source.Get("song_id");
			bpm = (float)_source.Get("bpm");
			offset = (float)_source.Get("offset");
			beats_per_tact = (int)_source.Get("beats_per_tact");
			source = (AudioStreamMP3)_source.Get("source");
			tact_length = (60 / bpm) * beats_per_tact;
			
			var strengths = _source.Get("beat_strengths");
			Vector2[] beats = (Vector2[]) strengths;
			
			foreach (var beat in beats)
			{
				beat_strengths.Add(new Beat(beat.x * tact_length, beat.y));
			}
		}
		
		/// <summary>
		/// Starts playing this song in the specified player
		/// </summary>
		public void Play(AudioStreamPlayer player)
		{
			GD.Print("Playing song");
			player.Stream = source;
			player.Autoplay = true;
			player.Play(offset);
			lastPlayer = player;
		}

		/// <summary>
		/// Returns the current strength of the beat, based on how close it is to one. Can return ApplicationException
		/// in case this song is not being played at the moment.
		/// </summary>
		public float GetCurrentStrength()
		{
			if (lastPlayer == null || lastPlayer.Stream != source)
			{
				throw new ApplicationException("Can't get beat strength");
			}
			float strength = 0;
			float currentOffset = (lastPlayer.GetPlaybackPosition() - offset) % tact_length;

			foreach (Beat beat in beat_strengths)
			{
				strength = Math.Max(beat.calculateStrength(currentOffset), strength);
			}

			return strength;
		}
	}
	
	/// <summary>
	/// Reloads songs from the Godot resource. If you want to add songs, add a song there then call this method.
	/// If autoLoad is true, it also tries to load all resources in res://src/playlist/assets/ as songs (not recursive)
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
			do {
				filename = dir.GetNext();
				if (filename.EndsWith(".tres"))
				{
					try
					{
						var newSong = new GameSong(ResourceLoader.Load(path + filename));
						GameSongs.Add(newSong.song_id, newSong);
					}
					catch (Exception e)
					{
						GD.PrintErr("Song resource invalid, autoload skipping.\nSong: " + path + filename + "\nReason: " + e.Message);
					}
				}
			} while (filename != "");
		}
		
		foreach (var _song in _songs)
		{
			var newSong = new GameSong(_song);
			GameSongs[newSong.song_id] = newSong;
		}
	}

	/// <summary>
	/// Starts playing the song specified by id, return the GameSong object
	/// </summary>
	public GameSong PlaySong(string id)
	{
		var toPlay = GameSongs[id];
		toPlay.Play(player);
		return toPlay;
	}

	public override void _Ready()
	{
		ReloadSongs(_auto_song_load);
		player = GetNode<AudioStreamPlayer>(_player);
	}
}
