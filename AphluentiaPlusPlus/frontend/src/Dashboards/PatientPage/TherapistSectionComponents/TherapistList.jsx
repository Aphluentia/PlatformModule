import {React, useState, useEffect} from 'react';
import './Css/TherapistList.css';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import isBase64 from 'is-base64';
import Button from '@mui/material/Button';

const TherapistList = ({ therapists, handleTherapistRequest }) => {
  const [list, setList] = useState(therapists);
  const [query, setQuery] = useState('');
  if (therapists != list && query ==='' ){
    setList(therapists);
  }

  const handleInputChange = (e) => {
    const inputValue = e.target.value;
    setQuery(inputValue);
    let filteredList = therapists.filter(item =>
      item.email.toLowerCase().includes(inputValue.toLowerCase())
      || item.firstName.toLowerCase().includes(inputValue.toLowerCase())
      || item.lastName.toLowerCase().includes(inputValue.toLowerCase())
    );
    if (inputValue === '') {
     filteredList = therapists;
    }
    
    setList(filteredList);
   
  };
  

  return (
    <div
      className="therapist-list-section-container">
      <div style={{ width: '90%', marginBottom:'5%',}}>
        <h2>Available Therapists</h2>
        <input
          type="text"
          placeholder="Search..."
          value={query}
          onChange={handleInputChange}
        />
        <div style={{ overflowY: 'auto' }}>
        
        <ul className="therapist-list">
          {
            list.length == 0 ?<p>No Available Therapists</p>: <p></p>
          }
          {list.map((therapist, index) => (
            <li key={index} style={{ margin:'1px', border: '1px solid lightgrey', display: 'flex'}}>            
              <div style={{ padding:"2%", display:'flex'}}>
                <div  style={{ width: '70px',   padding:'1%'}}>
                  {isBase64(therapist.profilePicture.split(",")[1]) ? (
                      <img
                        src={therapist.profilePicture}
                        alt="Profile"
                        style={{ height: 'auto',  maxWidth: '70px'}}
                        onClick={(e) => e.preventDefault()}
                      />
                    ):(
                      <p>Image Invalid</p>
                    )
                    }
                </div>
                <div style={{ maxWidth:'60%',   padding:'1%'}}>
                  <p><strong>{therapist.firstName} {therapist.lastName}</strong> </p>
                  <p>{therapist.email}</p>
                  <p><strong>Age:</strong> {therapist.age}</p>
                  <p><strong>Description:</strong> {therapist.description}</p>
                  <p> <strong>Credentials:</strong> {therapist.credentials}</p>
                </div>
              </div>
              <div style={{ display:'flex', maxWidth:'30%' }}>
                <div style={{padding:"2%" }}>
                  <Button  variant="outlined" color="success"
                    startIcon={<AddCircleOutlineIcon />} 
                    onClick={() => handleTherapistRequest(therapist.email)}>Add</Button>
                </div>
              </div>
            </li>
          ))}
        </ul>
      </div>
      </div>
    </div>
  );
};

export default TherapistList;