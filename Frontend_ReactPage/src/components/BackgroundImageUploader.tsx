import React from 'react';

interface Props {
  currentUrl?: string;
  onUpload: (url: string) => void;
}

const BackgroundImageUploader = ({ currentUrl, onUpload }: Props) => {
  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const url = URL.createObjectURL(file); 
      onUpload(url);
      // Später echten Upload implementieren!
    }
  };

  return (
    <div>
      {currentUrl && <img src={currentUrl} alt="Aktuelles Hintergrundbild" className="w-32 h-auto" />}
      <input type="file" accept="image/*" onChange={handleFileChange} />
    </div>
  );
};

export default BackgroundImageUploader;
