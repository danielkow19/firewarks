@tool
extends EditorPlugin


func _enter_tree():
	# Initialization of the plugin goes here.
	# Godot tries to auto indent on build and save, which is messing with out git files
	# This is an editor instead of project setting, so not saved in github, so I'm using a plugin script instead
	get_editor_interface().get_editor_settings().set_setting("text_editor/behavior/files/convert_indent_on_save", false)
	pass


func _exit_tree():
	# Clean-up of the plugin goes here.
	pass
