import React, { useState, useEffect } from 'react';
import axios from 'axios';
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import Button from '@mui/material/Button';
import isBase64 from 'is-base64';
import './ProfilePage.css'
import Loading from '../Base/Loading';



const ProfilePage = () => {
    const [user, setUser] = useState('');
    const [loading, setLoading] = useState(user === '');
    const [edit, setEdit] = useState(false);
    const token = localStorage.getItem('Token');
    const userType = localStorage.getItem('UserType');

    useEffect(() => {
        if (userType == 0 ? getPatientData() : getTherapistData());
    }, []);

    const getTherapistData= ()=>{
        
        setLoading(true);
        axios
        .get("https://localhost:7176/api/Therapist/" + token)
        .then(data => {
            setUser(data.data.data);
            
            setLoading(false);
        })
        .catch(error => {
            // Handle error
        });
    }
    const getPatientData= ()=>{
        
        setLoading(true);
        axios
        .get("https://localhost:7176/api/Patient/" + token)
        .then(data => {
            setUser(data.data.data);
            
            setLoading(false);
        })
        .catch(error => {
            // Handle error
        });
    }
    const saveChanges=()=>{
        setLoading(true);
        let url = "https://localhost:7176/api/";
        console.log(user);
            userType == 0 ? url+=("Patient/"+token) : url+=("Therapist/"+token) ;
                axios
                    .put(url, user)
                    .then(() => {
                        setEdit(false);

                        if (userType == 0 ? getPatientData() : getTherapistData());
                        
                        setLoading(false);
                        })
                    .catch(error => {
                    // Handle error
                    });
    }

    function changeMode() {
        setEdit(!edit);
    }

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setUser({
      ...user,
      [name]: value,
    });
  };
  function handleFileChange(event) {
    setLoading(true);
      let reader = new FileReader();
      reader.readAsDataURL( event.target.files[0]);
        reader.onload = function () {
          setUser({
            ...user,
            ['profilePicture']:reader.result,
          });
          
        setLoading(false);
      };
      reader.onerror = function (error) {
          console.log('Error: ', error);
      };
     
      
  }
  
  
 
return (<>
        <div className="profile-page">
        {loading == true ? 
            <Loading/>
            : (
            <>
            <div className="profile-picture">
                <span>
                    {user.profilePicture!=undefined && isBase64(user.profilePicture.split(",")[1]) ? (
                    <img
                        src={user.profilePicture}
                        alt="Profile"
                        style={{ maxWidth: '100px' }}
                        onClick={(e) => e.preventDefault()}
                    />
                    ):(
                    <p>Image Invalid</p>
                    )
                    }
                </span>
                {edit? <p> <input className="upload-file-button" type="file" onChange={handleFileChange} /></p> : <></>}
                
            </div>

            <div>
                <h2>
                {edit ? (
                    <input
                    type="text"
                    name="firstName"
                    value={user.firstName}
                    onChange={handleInputChange}
                    />
                ) : (
                    <span>
                    {user.firstName}
                    </span>
                )}{' '}
                {edit ? (
                    <input
                    type="text"
                    name="lastName"
                    value={user.lastName}
                    onChange={handleInputChange}
                    />
                ) : (
                    <span>
                    {user.lastName}
                    </span>
                )}
                </h2>
                <p>
                <b>Email:</b> {' '}
                    <span >{user.email}</span>
                
                </p>
                <p>
                <b>Age:</b>{' '}
                    {edit ? (
                    <input
                        type="number"
                        name="Age"
                        value={user.age}
                        onChange={handleInputChange}
                    />
                    ) : (
                    <span>{user.age}</span>
                    )}
                </p>
                <p>
                <b>Address:</b>{' '}
                    {edit ? (
                    <input
                        type="text"
                        name="address"
                        value={user.address}
                        onChange={handleInputChange}
                    />
                    ) : (
                    <span>{user.age}</span>
                    )}
                </p>
                <p>
                <b>Phone Number:</b>{' '}
                    {edit ? (
                    <input
                        type="text"
                        name="phoneNumber"
                        value={user.phoneNumber}
                        onChange={handleInputChange}
                    />
                    ) : (
                    <span>{user.age}</span>
                    )}
                </p>
                {userType == 1 ? 
                <>
                    <p>
                        <b>Credentials:</b>{' '}
                        {edit ? (
                            <textarea 
                            type="text"
                            name="credentials"
                            value={user.credentials}
                            onChange={handleInputChange}
                            />
                        ) : (
                            <span>{user.credentials}</span>
                        )}
                        
                        </p>
                        <p>
                        <b>Description:</b>{' '}
                        {edit ? (
                            <textarea 
                            type="text"
                            name="description"
                            value={user.description}
                            onChange={handleInputChange}
                            />
                        ) : (
                            <span>{user.description}</span>
                        )}
                    </p>
                </>:<>
                    <p>
                        <b>Condition Name:</b>{' '}
                        {edit ? (
                            <input 
                            type="text"
                            name="conditionName"
                            value={user.conditionName}
                            onChange={handleInputChange}
                            />
                        ) : (
                            <span>{user.conditionName}</span>
                        )}
                        
                        </p>
                        <p>
                        <b>Condition Acquisition Date:</b>{' '}
                        {edit ? (
                            <input 
                            type="date"
                            name="conditionAcquisitionDate"
                            value={user.conditionAcquisitionDate}
                            onChange={handleInputChange}
                            />
                        ) : (
                            <span>{user.conditionAcquisitionDate}</span>
                        )}
                    </p>
                </>
                }
            
                {edit == true ? ( 
                <div className='Buttons'>
                    <Button  variant="contained"
                    startIcon={<SaveOutlinedIcon />} 
                    onClick={() => saveChanges()}>Save</Button>
                    
                   
                </div>
                ):

                   
                <p> <input className="edit-button" type="button" onClick={changeMode} value="Edit"/></p>
                }
                
            

            </div>
            </>

        )}
        </div>
        </>
  );
};

export default ProfilePage;
