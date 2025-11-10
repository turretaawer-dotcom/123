import sys
import json
import math
import pyperclip
from math import degrees
import logging
from scipy.spatial.transform import Rotation as R



# Set up logging
logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')

# /Game/Environment/Rocks/



rockDictionary = {
    #  Format is ID : UnrealEngineName--UnrealEngineLocation--Scale--tag(noRot=no rotation, BP=Blueprint, clf=cliffSpecificChanges)
     3788869464 : "cliff_double_height_a--/Game/Environment/Rocks/--.01",
     868337454  : "cliff_double_height_b--/Game/Environment/Rocks/--.01",
     3201698096 : "cliff_double_height_c--/Game/Environment/Rocks/--.01",
     2538614362 : "cliff_double_height_d--/Game/Environment/Rocks/--.01",
     261440689  : "cliff_low_arc_arid--/Game/Environment/Rocks/--.01",
     1234854666 : "cliff_low_arc--/Game/Environment/Rocks/--.01",
     3901569408 : "cliff_low_sloped_L--/Game/Environment/Rocks/--.01",
     2929264923 : "cliff_low_sloped_L_arid--/Game/Environment/Rocks/--.01",
     1737990122 : "cliff_low_sloped_R--/Game/Environment/Rocks/--.01",
     1507611726 : "cliff_low_sloped_R_arid--/Game/Environment/Rocks/--.01",
     3284028296 : "cliff_medium_arc--/Game/Environment/Rocks/--.01",
     183291403  : "cliff_medium_arc_arid--/Game/Environment/Rocks/--.01",
     2998099425 : "cliff_medium_sloped_L--/Game/Environment/Rocks/--.01",
     459301333  : "cliff_medium_sloped_L_arid--/Game/Environment/Rocks/--.01",
     2001138413 : "cliff_medium_sloped_R--/Game/Environment/Rocks/--.01",
     1839093796 : "cliff_medium_sloped_R_arid--/Game/Environment/Rocks/--.01",
     2358884308 : "cliff_tall_arc--/Game/Environment/Rocks/--.01",
     188099007  : "cliff_tall_arc_arid--/Game/Environment/Rocks/--.01",
     1797393989 : "cliff_tall_pinch_a--/Game/Environment/Rocks/--.01",
     4197036578 : "cliff_tall_pinch_a_arid--/Game/Environment/Rocks/--.01",
     3973597035 : "cliff_tall_pinch_b--/Game/Environment/Rocks/--.01",
     2417746694 : "cliff_tall_pinch_b_arid--/Game/Environment/Rocks/--.01",
     3179789097 : "cliff_triple_height_a--/Game/Environment/Rocks/--.01",
     4272877361 : "cliff_triple_height_b--/Game/Environment/Rocks/--.01",
     214336632  : "rock_formation_medium_a--/Game/Environment/Rocks/--.01",
     1651904754 : "rock_formation_medium_b--/Game/Environment/Rocks/--.01",
     4276644322 : "rock_formation_medium_c--/Game/Environment/Rocks/--.01",
     1854787556 : "rock_formation_medium_d--/Game/Environment/Rocks/--.01",
     140564594  : "rock_formation_medium_e--/Game/Environment/Rocks/--.01",
     2713318896 : "rock_formation_a--/Game/Environment/Rocks/--.01",
     2515908409 : "rock_formation_b--/Game/Environment/Rocks/--.01",
     276134416  : "rock_formation_c--/Game/Environment/Rocks/--.01",
     3297956486 : "rock_formation_d--/Game/Environment/Rocks/--.01",
     3806991594 : "rock_formation_e--/Game/Environment/Rocks/--.01",
     291142845  : "rock_formation_f--/Game/Environment/Rocks/--.01",
     3526887621 : "ice_sheet_large_1--/Game/Environment/Ice/--.01",
     2092812127 : "ice_sheet_large_2--/Game/Environment/Ice/--.01",
     542703589  : "ice_sheet_large_3--/Game/Environment/Ice/--.01",
     563950422  : "ice_sheet_med_1--/Game/Environment/Ice/--.01",
     2190701600 : "ice_sheet_med_2--/Game/Environment/Ice/--.01",
     3805015439 : "ice_sheet_small_1--/Game/Environment/Ice/--.01",
     2795178101 : "ice_sheet_small_2--/Game/Environment/Ice/--.01",
     652326299  : "ice_sheet_small_3--/Game/Environment/Ice/--.01",
     2284707139 : "ice_sheet_small_4--/Game/Environment/Ice/--.01",
     2741295605 : "shore_ice_a--/Game/Environment/Ice/--.01",
     2175806731 : "shore_ice_b--/Game/Environment/Ice/--.01",
     506347060  : "shore_ice_c--/Game/Environment/Ice/--.01",
     2739477726 : "ice_lake_1--/Game/Environment/Rocks/--1--BP",
     2844422939 : "ice_lake_2--/Game/Environment/Rocks/--1--BP",
     944262640  : "ice_lake_3--/Game/Environment/Rocks/--1--BP",
     3242506542 : "ice_lake_4--/Game/Environment/Rocks/--1--BP",
     2304026153 : "powerline_pole_a--/Game/Environment/roads/telephone_poles/--1--noRot",
     3101594928 : "powerline_a--/Game/Environment/Misc/--1--noRot",
     4033141811 : "powerline_b--/Game/Environment/Misc/--1--noRot",
     3572220154 : "powerline_c--/Game/Environment/Misc/--1--noRot",
     162783028  : "powerline_d--/Game/Environment/Misc/--1--noRot",
     2701343650 : "powerline_a--/Game/Environment/Misc/--1--noRot", # Platform Variants
     1636154180 : "powerline_b--/Game/Environment/Misc/--1--noRot", # Platform Variants
     2257766461 : "powerline_c--/Game/Environment/Misc/--1--noRot", # Platform Variants
     1556711445 : "sewer_branch_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     1873767918 : "fishing_village_a_rootA_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     3605784707 : "fishing_village_b_rootB_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     179019713  : "fishing_village_c_rootC_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     779654519  : "ferry_terminal_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     3348191966 : "harbor_1_root1_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2958463062 : "harbor_2_root2_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     3968358155 : "arctic_research_base_a_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     4254972482 : "power_sub_big_1_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2915130455 : "power_sub_big_2_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     4110266940 : "power_sub_small_1_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     1816998299 : "power_sub_small_2_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     4273840478  : "lighthouse_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2720666271 : "airfield_1_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     72186600   : "military_tunnel_1_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     158704173  : "powerplant_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
    #  106246127  : "trainyard_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2200048243 : "junkyard_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     454470717  : "nuclear_missile_silo_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     1073114437 : "launch_site_BP--/Game/Environment/Monuments/HLODs/--1--BP",
     3257319375 : "sewer_branch_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     # 3965959566 : "desert_military_base_a_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2839094107 : "oilrig_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2038914978 : "oilrig_small_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     973576591  : "gas_station_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     400079324  : "radtown_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     4255620866 : "supermarket_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     891304283  : "warehouse_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     182769745  : "satellite_dish_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     3903103539 : "sphere_tank_Dome_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2250041975 : "stables_a_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     2973946649 : "stables_b_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     1879405026 : "compound_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     4273840478 : "trainyard_root_HLOD_mesh--/Game/Environment/Monuments/HLODs/--1--noRot",
     1309495556 : "ExcavatorV1_Blueprint--/Game/Environment/Monuments/Large_Excavator/--1--BP",
     3422899798 : "water_treatment_plant_hlod--/Game/Environment/Monuments/HLODs/--1--noRot",
     2495573051 : "jungle_ruins_a_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
     3485432612 : "jungle_ruins_b_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
     990927492  : "jungle_ruins_c_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
     2063757536 : "jungle_ruins_d_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
     526021709  : "jungle_ruins_e_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
     3810520938 : "jungle_ziggurat_a_HLOD--/Game/Environment/Monuments/HLODs/--1--noRot",
    
     1657861052 : "cliff_hills_large_a--/Game/Environment/Rocks/--1",
     3529635362 : "cliff_hills_large_b--/Game/Environment/Rocks/--1",
     1606325609 : "cliff_hills_large_c--/Game/Environment/Rocks/--1",
     3024342991 : "cliff_hills_large_d--/Game/Environment/Rocks/--1",
     3156217868 : "cliff_hills_large_e--/Game/Environment/Rocks/--1",
     2766539350 : "cliff_hills_large_f--/Game/Environment/Rocks/--1",
     3855233086 : "cliff_hills_mid_a--/Game/Environment/Rocks/--1",
     4064506111 : "cliff_hills_mid_b--/Game/Environment/Rocks/--1",
     3460557017 : "cliff_hills_mid_c--/Game/Environment/Rocks/--1",
     2108701271 : "cliff_hills_mid_d--/Game/Environment/Rocks/--1",
     1938976908 : "cliff_jutting_a--/Game/Environment/Rocks/--1--BP",
     3049282811 : "cliff_jutting_b--/Game/Environment/Rocks/--1--BP",
     342332366  : "cliff_jutting_d--/Game/Environment/Rocks/--1--BP",
     364300142  : "cliff_jutting_a--/Game/Environment/Rocks/--1--BP", # Snow Version
     3998037335 : "cliff_jutting_c--/Game/Environment/Rocks/--1--BP", # Snow Version
     2631061500 : "cliff_jutting_d--/Game/Environment/Rocks/--1--BP", # Snow Version
     3464059523 : "cliff_tall_a--/Game/Environment/Rocks/--1--clf",
     835824009  : "cliff_tall_b--/Game/Environment/Rocks/--1--clf",
     3043824457 : "cliff_tall_c--/Game/Environment/Rocks/--1--clf",
     1309014915 : "cliff_tall_d--/Game/Environment/Rocks/--1--clf",
     3990389305 : "cliff_tall_slope_l_a--/Game/Environment/Rocks/--1--clf",
     4006178246 : "cliff_tall_slope_l_b--/Game/Environment/Rocks/--1--clf",
     1698435108 : "cliff_tall_slope_r_a--/Game/Environment/Rocks/--1--clf",
     4080196877 : "cliff_tall_slope_r_b--/Game/Environment/Rocks/--1--clf",
     2183677438 : "coastal_rocks_a--/Game/Environment/Rocks/--1--BP",
     2313356593 : "coastal_rocks_b--/Game/Environment/Rocks/--1--BP",
     2215511226 : "coastal_rocks_c--/Game/Environment/Rocks/--1--BP",
     4025698377 : "coastal_rocks_d--/Game/Environment/Rocks/--1--BP",
     438446188  : "coastal_rocks_e--/Game/Environment/Rocks/--1--BP",
     3195658030 : "coastal_rocks_large_a--/Game/Environment/Rocks/--1--BP",
     2534567777 : "coastal_rocks_large_b--/Game/Environment/Rocks/--1--BP",
     1220149082 : "coastal_rocks_large_c--/Game/Environment/Rocks/--1--BP",
     1591169581 : "coastal_rocks_large_d--/Game/Environment/Rocks/--1--BP",
     4072435111 : "coastal_rocks_large_e--/Game/Environment/Rocks/--1--BP",
     1840273428 : "coastal_rocks_large_a--/Game/Environment/Rocks/--1--BP",
     3500620592 : "coastal_rocks_large_b--/Game/Environment/Rocks/--1--BP",
     2971042728 : "coastal_rocks_large_c--/Game/Environment/Rocks/--1--BP",
     728010131  : "coastal_rocks_large_e--/Game/Environment/Rocks/--1--BP",
     3830715959 : "formation_large_arid_a--/Game/Environment/Rocks/--1--BP",
     2506672559 : "formation_large_arid_b--/Game/Environment/Rocks/--1--BP",
     3631964820 : "formation_medium_arid_a--/Game/Environment/Rocks/--.01", 
     1077083573 : "formation_medium_arid_b--/Game/Environment/Rocks/--.01", 
     2939550396 : "formation_medium_arid_c--/Game/Environment/Rocks/--.01", 
     732484098  : "formation_medium_arid_d--/Game/Environment/Rocks/--.01", 
     2713318896 : "rock_formation_a_new--/Game/Environment/Rocks/--1--BP",
     2515908409 : "rock_formation_b_new--/Game/Environment/Rocks/--1--BP",
     276134416  : "rock_formation_c_new--/Game/Environment/Rocks/--1--BP",
     3297956486 : "rock_formation_d_new--/Game/Environment/Rocks/--1--BP",
     3806991594 : "rock_formation_e_new--/Game/Environment/Rocks/--1--BP",
     291142845  : "rock_formation_f_new--/Game/Environment/Rocks/--1--BP",
     2263966948 : "rock_formation_g_new--/Game/Environment/Rocks/--1--BP",
     1737704643 : "rock_formation_h_new--/Game/Environment/Rocks/--1--BP",
     294952495  : "rock_formation_i_new--/Game/Environment/Rocks/--1--BP",
     598565442  : "rock_formation_j_new--/Game/Environment/Rocks/--1--BP",
     654103635  : "cave_large_hard--/Game/Environment/Rocks/Caves/--1--BP",
     2624480487 : "cave_large_medium--/Game/Environment/Rocks/Caves/--1--BP", 
     1556711445 : "cave_large_sewers_hard--/Game/Environment/Rocks/Caves/--1--BP",
     1547444377 : "cave_medium_easy--/Game/Environment/Rocks/Caves/--1--BP",
     3870020960 : "cave_medium_hard--/Game/Environment/Rocks/Caves/--1--BP",
     2203204346 : "cave_medium_medium--/Game/Environment/Rocks/Caves/--1--BP",
     198044338  : "cave_small_easy--/Game/Environment/Rocks/Caves/--1--BP",
     2838540317 : "cave_small_hard--/Game/Environment/Rocks/Caves/--1--BP",
     1017425168 : "cave_small_medium--/Game/Environment/Rocks/Caves/--1--BP",

}


# You can now use asset_dump_content in your script as needed


def process_json_file(file_path):
    try:
        with open(file_path, 'r') as file:
            json_data = json.load(file)

        logging.info(f"Loaded JSON data from {file_path}")

        # Empty list to fill with UE actors data for the selected objects
        actorsList = []

        for prefab in json_data['rePrefab']['prefabs']:
            try:
                ID = prefab['id']
                
                if ID in rockDictionary:
                    namePath = rockDictionary[ID].split('--')

                    # Get object location
                    locX = prefab['position']['x']*100  # Flipped back
                    locY = -prefab['position']['z']*100  # Negated for 180 rotation
                    locZ = prefab['position']['y']*100 + 95  # Height offset included

                    # Convert Unity Euler angles to quaternion (Unity uses YXZ rotation order)
                    unity_rot = R.from_euler('yxz', [
                        prefab['rotation']['y'],
                        prefab['rotation']['x'],
                        prefab['rotation']['z']
                    ], degrees=True)

                    # Convert to Unreal coordinate system
                    unreal_rot = R.from_quat([
                        unity_rot.as_quat()[0],
                        -unity_rot.as_quat()[2],
                        unity_rot.as_quat()[1],
                        unity_rot.as_quat()[3]
                    ])

                    # Get Unreal Euler angles (Unreal uses ZYX order: Yaw, Pitch, Roll)
                    unreal_euler = unreal_rot.as_euler('zyx', degrees=True)
                    rotZ = unreal_euler[0] + 180  # Yaw
                    rotY = unreal_euler[1]        # Pitch
                    rotX = unreal_euler[2] + 90   # Roll (adding 90 degrees local rotation)

                    # Get object scale (unchanged)
                    sclX = prefab['scale']['x'] * float(namePath[2])
                    sclY = prefab['scale']['y'] * float(namePath[2])
                    sclZ = prefab['scale']['z'] * float(namePath[2])

                    # Extra Tag cases
                    if len(namePath) > 3:
                        match namePath[3]:
                            case "clf":
                                # Update Transforms for v3Cliffs
                                locZ += -4700
                                rotZ += 90
                                # Translate "clf" meshes in local space on the x axis
                                local_offset_x = -2800
                                # Convert local offset to world space
                                world_offset_x = local_offset_x * math.cos(math.radians(rotZ))
                                world_offset_y = local_offset_x * math.sin(math.radians(rotZ))
                                # Apply the offset
                                locX += world_offset_x
                                locY += world_offset_y
                            case "noRot":
                                # Reset rotation for objects that shouldn't rotate
                                rotX = 0
                                rotY = 0
                                rotZ = 0
                            case _:
                                pass

                    if len(namePath) > 3 and namePath[3] == "BP":
                        actor_text = f"""
                           Begin Actor Class={namePath[1]}{namePath[0]}.{namePath[0]}_C Name={namePath[0]}_C Archetype="{namePath[1]}{namePath[0]}.{namePath[0]}_C'{namePath[1]}{namePath[0]}.Default__{namePath[0]}_C'" ExportPath="{namePath[1]}{namePath[0]}.{namePath[0]}_C'/Temp/Untitled_0.Untitled:PersistentLevel.{namePath[0]}_C_1'"
                              Begin Object Name="InstancedStaticMesh" ExportPath="/Script/Engine.InstancedStaticMeshComponent'/Temp/Untitled_0.Untitled:PersistentLevel.{namePath[0]}_Blueprint_C_1.InstancedStaticMesh'"
                                 RelativeLocation=(X={locX},Y={locY},Z={locZ})
                                 RelativeRotation=(Pitch={rotY},Yaw={rotZ},Roll={rotX-90})
                                 UCSSerializationIndex=0
                                 bNetAddressable=True
                                 CreationMethod=SimpleConstructionScript
                              End Object
                           End Actor"""
                    else:
                        ue_static_mesh_path = namePath[1] + namePath[0] + '.' + namePath[0]
                        actor_text = f"""
                            Begin Actor Class=/Script/Engine.StaticMeshActor Name=Cube19_4 Archetype=/Script/Engine.StaticMeshActor'/Script/Engine.Default__StaticMeshActor'
                                Begin Object Class=/Script/Engine.StaticMeshComponent Name="StaticMeshComponent0" Archetype=/Script/Engine.StaticMeshComponent'/Script/Engine.Default__StaticMeshActor:StaticMeshComponent0'
                                End Object
                                Begin Object Name="StaticMeshComponent0"
                                    StaticMesh={ue_static_mesh_path}
                                    RelativeLocation=(X={locX},Y={locY},Z={locZ})
                                    RelativeRotation=(Pitch={rotY},Yaw={rotZ},Roll={rotX})
                                    RelativeScale3D=(X={sclX},Y={sclY},Z={sclZ})
                                End Object
                                StaticMeshComponent="StaticMeshComponent0"
                                RootComponent="StaticMeshComponent0"
                                ActorLabel="{str(namePath[0])}"
                            End Actor"""
                    
                    actorsList.append(actor_text)
                    logging.debug(f"Added actor for {namePath[0]}")
            except Exception as e:
                logging.error(f"Error processing prefab: {e}")
        
        # Join the actors text
        actorsText = " ".join(actorsList)

        # Text snippet that goes at the beginning
        beginText = """Begin Map
            Begin Level"""
        # Text snippet that goes at the end
        endText = """
            End Level
        Begin Surface
        End Surface
        End Map"""

        full_text = beginText + actorsText + endText
        pyperclip.copy(full_text)
        logging.info("Actors copied to clipboard successfully")
        return True

    except Exception as e:
        logging.error(f"An error occurred: {e}")
        return False

def main():
    print("Unity JSON to Unreal Scene Converter")
    print("------------------------------------")
    print("Please drag and drop your JSON file into this window and press Enter.")
    print("Or type the full path to your JSON file and press Enter.")
    print("Type 'exit' to quit the program.")
    
    while True:
        file_path = input("\nEnter file path: ").strip()
        
        if file_path.lower() == 'exit':
            break
            
        # Remove quotes if present (common when dragging and dropping)
        file_path = file_path.strip('"')
        
        if process_json_file(file_path):
            print("\nProcessing complete! The data has been copied to your clipboard.")
        else:
            print("\nFailed to process the file. Please check the error messages above.")
        
        print("\nYou can process another file or type 'exit' to quit.")

if __name__ == "__main__":
    main()









# Begin Map
#    Begin Level
#       Begin Actor Class=/Game/Environment/Ice/Berg_BPs/Ice_Berg_2_Blueprint.Ice_Berg_2_Blueprint_C Name=Ice_Berg_2_Blueprint_C_1 Archetype="/Game/Environment/Ice/Berg_BPs/Ice_Berg_2_Blueprint.Ice_Berg_2_Blueprint_C'/Game/Environment/Ice/Berg_BPs/Ice_Berg_2_Blueprint.Default__Ice_Berg_2_Blueprint_C'" ExportPath="/Game/Environment/Ice/Berg_BPs/Ice_Berg_2_Blueprint.Ice_Berg_2_Blueprint_C'/Temp/Untitled_0.Untitled:PersistentLevel.Ice_Berg_2_Blueprint_C_1'"
#          Begin Object Name="InstancedStaticMesh" ExportPath="/Script/Engine.InstancedStaticMeshComponent'/Temp/Untitled_0.Untitled:PersistentLevel.Ice_Berg_2_Blueprint_C_1.InstancedStaticMesh'"
#             RelativeLocation=(X=3622.441172,Y=12219.840251,Z=7337.274076)
#             RelativeRotation=(Pitch=6.408646,Yaw=-50.431313,Roll=-7.692629)
#             UCSSerializationIndex=0
#             bNetAddressable=True
#             CreationMethod=SimpleConstructionScript
#          End Object
#       End Actor
#    End Level
# Begin Surface
# End Surface
# End Map