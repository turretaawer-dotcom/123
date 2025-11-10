import os
import re
from PIL import Image
import numpy as np

def create_combined_mask(input_image_path, output_directory):
    """
    Reads an RGBA image containing a 32-bit integer bitmask and creates a
    single new RGBA image with Layer 6 in the Red channel and Layer 15
    in the Green channel.
    """
    print("-" * 50)
    print(f"Processing image for combined mask: {input_image_path}")
    
    try:
        # Create the output directory if it doesn't exist
        if not os.path.exists(output_directory):
            os.makedirs(output_directory)
            print(f"Created output directory: {output_directory}")

        # Open the source image and ensure it's in RGBA format
        img = Image.open(input_image_path).convert('RGBA')
        
        # Convert the image to a NumPy array for fast processing.
        image_array = np.array(img, dtype=np.uint32)
        
        # Decode the 32-bit integer mask from the RGBA channels
        r, g, b, a = image_array[:, :, 0], image_array[:, :, 1], image_array[:, :, 2], image_array[:, :, 3]
        decoded_mask = (a << 24) | (b << 16) | (g << 8) | r
        print("Successfully decoded bitmask from image.")

    except Exception as e:
        print(f"ERROR: Could not load or process the image. Reason: {e}")
        return

    # --- Create the new combined RGBA image ---
    
    # Get the height and width from the decoded mask shape
    height, width = decoded_mask.shape
    
    # Create an empty (all black) 4-channel RGBA array for our output
    # The shape is (height, width, 4 channels)
    output_rgba_array = np.zeros((height, width, 4), dtype=np.uint8)

    # --- Process Layer 6 for the RED channel ---
    print("Processing Layer 6 for RED channel...")
    layer_6_mask = (decoded_mask & (1 << 6)) != 0
    # Where the mask is True, set the Red channel (index 0) to 255
    output_rgba_array[layer_6_mask, 0] = 255

    # --- Process Layer 15 for the GREEN channel ---
    print("Processing Layer 15 for GREEN channel...")
    layer_15_mask = (decoded_mask & (1 << 15)) != 0
    # Where the mask is True, set the Green channel (index 1) to 255
    output_rgba_array[layer_15_mask, 1] = 255
    
    # The Blue channel (index 2) is already 0.
    # We will set the Alpha channel (index 3) to 255 (fully opaque) for compatibility.
    output_rgba_array[:, :, 3] = 255

    # --- Save the single combined image ---
    print("Saving combined mask image...")
    # Convert the 4-channel NumPy array back into an RGBA image
    output_image = Image.fromarray(output_rgba_array, mode='RGBA')
    output_filename = "combined_mask_6R_15G.png"
    output_path = os.path.join(output_directory, output_filename)
    output_image.save(output_path)

    # --- The old loop for creating individual maps is now commented out ---
    """
    for layer_index in range(32):
        check_mask = 1 << layer_index
        layer_is_active = (decoded_mask & check_mask) != 0

        if not np.any(layer_is_active):
            continue

        print(f"  -> Layer {layer_index} has data. Generating and saving map...")
        
        output_array = np.zeros(decoded_mask.shape, dtype=np.uint8)
        output_array[layer_is_active] = 255

        output_image = Image.fromarray(output_array, mode='L')
        output_filename = f"layer_{layer_index}_mask.png"
        output_path = os.path.join(output_directory, output_filename)
        output_image.save(output_path)
    """
    
    print("-" * 50)


def find_and_process_latest_topology():
    """
    Finds the correct folder and 'Topology.png' file based on the specified
    rules and then processes it.
    """
    # ==================================================================
    # ===                EDIT THIS VARIABLE                        ===
    ROOT_SEARCH_DIRECTORY = "O:\.RustProjects"
    # ==================================================================

    if not os.path.isdir(ROOT_SEARCH_DIRECTORY):
        print(f"ERROR: The specified root directory does not exist: '{ROOT_SEARCH_DIRECTORY}'")
        return

    highest_number = -1
    target_folder = None

    print(f"Scanning '{os.path.abspath(ROOT_SEARCH_DIRECTORY)}' for target folders...")
    for item in os.listdir(ROOT_SEARCH_DIRECTORY):
        item_path = os.path.join(ROOT_SEARCH_DIRECTORY, item)
        if os.path.isdir(item_path):
            match = re.search(r'(\d+)$', item)
            if match:
                number = int(match.group(1))
                if number > highest_number:
                    highest_number = number
                    target_folder = item_path
    
    if not target_folder:
        print(f"ERROR: Could not find any folders ending with a number.")
        return

    print(f"Found highest numbered folder: '{target_folder}'")

    topology_image_path = None
    topology_parent_directory = None
    
    for root, dirs, files in os.walk(target_folder):
        for file in files:
            if file.lower() == 'topology.png':
                topology_image_path = os.path.join(root, file)
                topology_parent_directory = root
                break
        if topology_image_path:
            break

    if not topology_image_path:
        print(f"ERROR: Could not find 'Topology.png' inside '{target_folder}'.")
        return

    print(f"Found topology file at: '{topology_image_path}'")
    
    output_dir_path = os.path.join(topology_parent_directory, 'topology')
    
    # Call the new processing function
    create_combined_mask(topology_image_path, output_dir_path)

    print("\nScript finished successfully!")


if __name__ == "__main__":
    find_and_process_latest_topology()