import pyperclip

# Get the text from the clipboard
clipboard_text = pyperclip.paste()

# Replace "metal" with "container"
modified_text = clipboard_text.replace("metal", "container")
modified_text = modified_text.replace("container_embrasure", "metal_embrasure")
modified_text = modified_text.replace("barricade_container", "barricade_metal")

# Copy the modified text back to the clipboard
pyperclip.copy(modified_text)

print("Text in clipboard has been modified: 'metal' replaced with 'container'")



