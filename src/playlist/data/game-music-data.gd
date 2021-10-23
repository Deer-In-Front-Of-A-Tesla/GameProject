class_name GameMusic 
extends Resource; 

export(String) var name; 
export(String) var song_id; 
export(float) var offset; 
export(float) var bpm = 90; 
export(int) var beats_per_tact = 4;
export(AudioStreamMP3) var source;

# X is the offset from the start of a tact. 0 is the exact start, therefore a beat
#  X = 1 is the starting beat of the following tact, therefore X should be
#  0 <= X < 1
# Y is the coefficient for hitting the beat correctly/how important that beat is.
# for example [0, 1] means strength 1 for the starting beat
# [0.5, 0.5] means that the secondary beat is half as important
export(PoolVector2Array) var beat_strengths; 

var tact_length: float = (60/bpm) * beats_per_tact;
