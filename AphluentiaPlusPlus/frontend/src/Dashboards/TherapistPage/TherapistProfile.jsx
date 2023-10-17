import './TherapistProfile.css';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import Button from '@mui/material/Button';
import isBase64 from 'is-base64';



const TherapistProfile = () => {
  const [therapist, setTherapist] = useState({});
  const [initialTherapist, setInitialTherapist] = useState({});
  const [editableField, setEditableField] = useState(null);
  const [changed, setChanged] = useState(false);
  const token = localStorage.getItem('Authentication');
  const email = localStorage.getItem('Email');
  let [imgSrc,setImgSrc] = useState('')

  useEffect(() => {
     getTherapistData(token, email);
  }, []);

  const getTherapistData= ()=>{
    console.log("---------")
    axios
      .get("https://localhost:7176/api/Therapist/" + token + "/Email/" + email)
      .then(data => {
        setTherapist(data.data.data);
        setInitialTherapist(data.data.data)
      })
      .catch(error => {
        // Handle error
      });
  }
  const saveChanges=()=>{
    console.log("_--------------------")
      axios
        .put("https://localhost:7176/api/Therapist/" + token, therapist)
        .then(() => {
          getTherapistData();
        })
        .catch(error => {
          // Handle error
        });
      setChanged(false);
  }
  const handleFieldClick = (fieldName) => {
    
    console.log("_--------------------2")
    setEditableField(fieldName);
    setChanged(true);
    if (fieldName === null)
      saveChanges();
    console.log(therapist);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    console.log("_--------------------3")
    setTherapist({
      ...therapist,
      [name]: value,
    });
  };
  function handleFileChange(event) {
      let reader = new FileReader();
      reader.readAsDataURL( event.target.files[0]);
        reader.onload = function () {
          console.log(reader.result);
          setTherapist({
            ...therapist,
            ['profilePicture']:reader.result,
          });
          setChanged(true);
      };
      reader.onerror = function (error) {
          console.log('Error: ', error);
      };
     
      
  }
  
  
 
return (
    <div className="therapist-profile">
     
      {therapist !== initialTherapist && changed === false ? <p>Loading...</p> : (
          <>
            <div className="profile-picture">
                  <span onClick={() => handleFieldClick('profilePicture')}>
                    {isBase64(therapist.profilePicture.split(",")[1]) ? (
                      <img
                        src={therapist.profilePicture}
                        alt="Profile"
                        style={{ width: '100%', height: 'auto', maxWidth: '100px' }}
                        onClick={(e) => e.preventDefault()}
                      />
                    ):(
                      <p>Image Invalid</p>
                    )
                    }
                    
                      
                  </span>
                  
          </div>

          <div className="profile-details">
            
                
            <h2>
              {editableField === 'firstName' ? (
                <input
                  type="text"
                  name="firstName"
                  value={therapist.firstName}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('firstName')}>
                  {therapist.firstName}
                </span>
              )}{' '}
              {editableField === 'lastName' ? (
                <input
                  type="text"
                  name="lastName"
                  value={therapist.lastName}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('lastName')}>
                  {therapist.lastName}
                </span>
              )}
            </h2>
            <p>
              <b>Email:</b> {' '}
             
                <span onClick={() => handleFieldClick('email')}>{therapist.email}</span>
              
            </p>
            <p>
              <b>Age:</b>{' '}
                {editableField === 'age' ? (
                  <input
                    type="number"
                    name="Age"
                    value={therapist.age}
                    onChange={handleInputChange}
                    onBlur={() => setEditableField(null)}
                  />
                ) : (
                  <span onClick={() => handleFieldClick('age')}>{therapist.age}</span>
                )}
            </p>
            <p>
            <b>Credentials:</b>{' '}
              {editableField === 'credentials' ? (
                <textarea 
                  type="text"
                  name="credentials"
                  value={therapist.credentials}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('credentials')}>{therapist.credentials}</span>
              )}
             
            </p>
            <p>
              <b>Description:</b>{' '}
              {editableField === 'description' ? (
                <textarea 
                  type="text"
                  name="description"
                  value={therapist.description}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('description')}>{therapist.description}</span>
              )}
            </p>
            {changed == true ? ( <Button  variant="contained"
              startIcon={<SaveOutlinedIcon />} 
              onClick={() => handleFieldClick(null)}>Save</Button>):
              <p></p>
            }
            
          <input type="file" onChange={handleFileChange} />
           

          </div>
          </>

      )}
    </div>
  );
};

export default TherapistProfile;
