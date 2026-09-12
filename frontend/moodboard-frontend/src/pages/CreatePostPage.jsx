import React, { useState } from "react";
import "../styles/CreatePostPage.css";
import SidebarComponent from "../components/SidebarComponent";
import SearchComponent from "../components/SearchComponent.jsx";

const Toggle = ({ checked, onChange, label }) => (
    <label className="toggle-row">
        <a
            type="button"
            role="switch"
            aria-checked={checked}
            aria-label={label}
            className={`toggle-switch${checked ? " is-on" : ""}`}
            onClick={() => onChange(!checked)}
        >
            <span className="toggle-switch-thumb" />
        </a>
        <span className="toggle-label">{label}</span>
    </label>
);

const CreatePostPage = () => {
    const [file, setFile] = useState(null);
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [tags, setTags] = useState("");
    const [isDragActive, setIsDragActive] = useState(false);

    const [isPrivate, setIsPrivate] = useState(true);
    const [allowComments, setAllowComments] = useState(true);
    const [allowDownload, setAllowDownload] = useState(true);

    const handleDrop = (e) => {
        e.preventDefault();
        setIsDragActive(false);
        if (e.dataTransfer.files?.[0]) {
            setFile(e.dataTransfer.files[0]);
        }
    };

    const handleFileSelect = (e) => {
        if (e.target.files?.[0]) {
            setFile(e.target.files[0]);
        }
    };

    return (
        <div className="gallery-layout">
            <SidebarComponent activeItem="user" />

            {/* Main Content */}
            <main className="main-content">
                <header className="header-1">
                    <div className="header-content-1">
                        <SearchComponent />
                        <div className="notification-btn">
                            <img src="/assets/icons/bell.png" alt="Notifications" />
                        </div>
                    </div>
                </header>

                <div className="with-scroll">
                    {/* Page heading */}
                    <div className="create-post-heading">
                        <a className="create-post-back" aria-label="Back">
                            <img className="arrow-back" src="/assets/icons/arrow-left-01.png" alt="" />
                        </a>
                        <h1>Create Post</h1>
                        <img
                            className="create-post-heading-caret"
                            src="/src/assets/images/arrow-left-01.png"
                            alt=""
                        />
                    </div>

                    <div className="create-post-body">
                        <div className="create-post-upload-column">
                            <label
                                className={`create-post-dropzone${isDragActive ? " is-active" : ""}`}
                                onDragOver={(e) => {
                                    e.preventDefault();
                                    setIsDragActive(true);
                                }}
                                onDragLeave={() => setIsDragActive(false)}
                                onDrop={handleDrop}
                            >
                                <input
                                    type="file"
                                    accept=".jpg,.jpeg,.mp4"
                                    onChange={handleFileSelect}
                                    hidden
                                />
                                <img
                                    className="create-post-upload-icon"
                                    src="/src/assets/images/upload-05.png"
                                    alt=""
                                />
                                <p className="create-post-dropzone-title">
                                    {file ? file.name : "Choose a file or drag and\nplace it here"}
                                </p>
                                <p className="create-post-dropzone-hint">
                                    We recommend using high quality .jpg files less
                                    <br />
                                    than 20 MB or .mp4 files less than 200 MB
                                </p>
                            </label>

                            <button type="button" className="create-post-url-btn">
                                Save from URL
                            </button>
                        </div>

                        <div className="create-post-form">
                            <div className="create-post-field">
                                <label htmlFor="post-title">Title</label>
                                <input
                                    id="post-title"
                                    type="text"
                                    placeholder="Add a catchy title..."
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                />
                            </div>

                            <div className="create-post-field">
                                <label htmlFor="post-description">Description</label>
                                <textarea
                                    id="post-description"
                                    placeholder="Tell everyone what your Post is about..."
                                    value={description}
                                    onChange={(e) => setDescription(e.target.value)}
                                    rows={4}
                                />
                            </div>

                            <div className="create-post-field">
                                <label htmlFor="post-tags">Tags</label>
                                <textarea
                                    id="post-tags"
                                    className="create-post-tags"
                                    placeholder="Add a catchy hashtags..."
                                    value={tags}
                                    onChange={(e) => setTags(e.target.value)}
                                    rows={2}
                                />
                            </div>

                            <a type="button" className="create-post-row">
                                <span>Board</span>
                                <img src="/src/assets/images/arrow-left-01.png" alt="" />
                            </a>

                            <a type="button" className="create-post-row create-post-more">
                                <span>More options</span>
                                <img src="/src/assets/images/arrow-left-01.png" alt="" />
                            </a>

                            <div className="create-post-toggles">
                                <Toggle
                                    checked={isPrivate}
                                    onChange={setIsPrivate}
                                    label="Make this post private"
                                />
                                <Toggle
                                    checked={allowComments}
                                    onChange={setAllowComments}
                                    label="Allow people to comment"
                                />
                                <Toggle
                                    checked={allowDownload}
                                    onChange={setAllowDownload}
                                    label="Allow people to download in their device"
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>
    );
};

export default CreatePostPage;
