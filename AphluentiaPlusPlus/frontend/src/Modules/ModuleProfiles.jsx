import React from 'react';
import {useContext, useState, useEffect} from 'react';
import './ModuleProfiles.css';

const ModuleProfiles = ({profilesList, currentProfile, updateCurrentProfile, createProfile, deleteProfile}) => {
    const [selectedProfile, setSelectedProfile] = useState(currentProfile)
    const [newProfileName, setNewProfileName] = useState('')
    useEffect(() => {
        
    }, []);

    const updateProfile = () => {
        updateCurrentProfile(selectedProfile);
    }
    const changeProfile = (event) => {
        const profile = event.target.value;
        console.log(profile);
        setSelectedProfile(profile);
    };
    const removeProfile = (event) => {
        deleteProfile(selectedProfile);
    };
    const createNewProfile = (event) => {
        createProfile(newProfileName);
        setNewProfileName('');
    };
    const changeNewProfileName = (event) => {
        const profile = event.target.value;
        setNewProfileName(profile);
    };

    return (<>
            <p className="profile-title"><b>Current Profile: {currentProfile}</b></p>
            <div>
                <select className="select-profile" onChange={changeProfile} value={selectedProfile}>
                    <option value=''>Select Profile</option>
                    {profilesList.map(profile => (
                        <option key={profile} value={profile}>
                            {profile}
                        </option>
                    ))}
                </select>
                <input 
                    className="set-profile-button"
                    type="button" 
                    value="Set as Current Profile"
                    onClick={updateProfile} 
                />
                <input 
                    className="remove-profile-button"
                    type="button" 
                    value="Remove Profile"
                    onClick={removeProfile} 
                />
            </div>
            <div> 
                <input 
                    className="new-profile-input"
                    type="text" 
                    placeholder='New Profile Name' 
                    onChange={changeNewProfileName}
                />
                <input 
                    className="new-profile-button"
                    type="button" 
                    value="Create New Profile"
                    onClick={createNewProfile} 
                />
            </div>
            </>
        
        )
    };

export default ModuleProfiles;
