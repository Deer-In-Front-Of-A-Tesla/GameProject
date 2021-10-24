using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public class playlist : Node2D
{
	[Export]
	public Resource[] _songs;

	[Export] 
	public NodePath _player;

	private System.Collections.Generic.Dictionary<string, GameSong> GameSongs =
		new System.Collections.Generic.Dictionary<string, GameSong>();
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

		public void Play(AudioStreamPlayer player)
		{
			GD.Print("Playing song");
			player.Stream = source;
			player.Autoplay = true;
			player.Play(offset);
			lastPlayer = player;
		}

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

	void ReloadSongs()
	{
		GameSongs.Clear();
		foreach (var _song in _songs)
		{
			var newSong = new GameSong(_song);
			GameSongs.Add(newSong.song_id, newSong);
		}
	}

	public GameSong PlaySong(string id)
	{
		var toPlay = GameSongs[id];
		toPlay.Play(player);
		return toPlay;
	}

	public override void _Ready()
	{
		ReloadSongs();
		player = GetNode<AudioStreamPlayer>(_player);
		
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		
	}
}
