const eventBus = new Vue;
new Vue({
    el: '#app',
    data: {
        announcementImage:"",
        Announcementes: [],
        message: 'Hello',
        tabs: {
            '首頁管理': { tabType: '首頁' },
            '輪播圖管理': { tabType: '輪播圖' },
            '優惠公告管理': { tabType: '優惠公告' },
            '熱賣管理': { tabType: '熱賣' },
        },
        toast: "",     
        EditInfo: {
            id: 1,
            Status: false,
            Context: "請輸入內容",
            Creater: "",
            Img: null,
            Type: "",
        },
        EditId: 0,
        Announcementes: [],
        oldImg: null,
        oldContext: "",
        oldCreater: "",
        oldStatus: null,
        oldType: "",       
        createInfo: {
            Id: 0,
            Status: false,
            Context: "",
            Creater: "",
            Img: null,
            Type: "",
        },
        uplodaImgPreview: {
            preview: null,
            image: null,
        },
        activeTab:'首頁管理',
        selectedType:"",
    },
    computed: {
        tabContent() {
            return this.tabs[this.activeTab];
        },      
    },
    methods: {
        CreateAA() {
            this.createInfo.Createtime = Date.now();
        },   
        setTabActive(tabName, tabType) {           
            this.activeTab = tabName;
            this.selectedType = tabType;
            eventBus.$emit('tabChanged', tabType);
        },
        GetAnouncementDetail() {
            let _this = this;
            axios.get("/BackStage/Announcement/GetAnnouncement").then(response => {
                _this.Announcementes = response.data
                eventBus.$emit('announcementesUpdated', AList);
            })       
        },
        //取消
        cancel() {
            //alert("cancel")
            let _this = this;
            var AAList = [];
            for (var i = 0; i < _this.Announcementes.length; i++) {
                var item = _this.Announcementes[i];
                if (item.changeEdit == true) {
                    item.Img = _this.oldImg;
                    item.Context = _this.oldContext;
                    item.Creater = _this.oldCreater;
                    item.Status = _this.oldStatus;
                    item.Type = _this.oldType;
                }
                AAList.push(item);
            }
            _this.Announcementes = AAList;
        },
        //編輯
        EditAnnouncement() {
            let _this = this;
            let item = _this.EditInfo;
            let formData = new FormData();

            formData.append("AnnouncementId", item.id);
            formData.append("AnnouncementImage", this.uplodaImgPreview.image);
            formData.append("AnnouncementContent", item.Context);
            formData.append("AnnouncementStatus", item.Status);
            formData.append("AnnouncementType", item.Type);
            formData.append("AnnouncementCreater", item.Creater);
            axios.post("/BackStage/Announcement/Edit", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then(response => {
                _this.GetAnouncementDetail();
                var modal = document.getElementById("staticBackdrop");
                var bsModal = bootstrap.Modal.getInstance(modal);
                bsModal.hide();
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'success',
                    title: '修改成功'
                })
            }).catch(error => {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: '修改失敗!欄位勿空白'
                })
            })         
        },
        //建立
        CreateAnnouncement() {
            let _this = this;
            let formData = new FormData();
            formData.append("AnnouncementContent", _this.createInfo.Context);
            formData.append("AnnouncementType", _this.createInfo.Type);
            formData.append("AnnouncementStatus", _this.createInfo.Status);
            formData.append("AnnouncementCreater", _this.createInfo.Creater);

            let fileInput = document.getElementById("fileInput");
            if (fileInput.files.length > 0) {
                formData.append("AnnouncementImage", fileInput.files[0]);
            }
            axios.post('/BackStage/Announcement/Create', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }                
            }).then(response => {
                _this.GetAnouncementDetail();
                var modal = document.getElementById("staticBackdrop");
                var bsModal = bootstrap.Modal.getInstance(modal);
                bsModal.hide(); 
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 1500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'success',
                    title: '新增成功'
                })
            }).catch(error => {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: '新增失敗!欄位勿空白'
                })
            })          
        },
        previewImage: function (e) {
            var input = e.target;
            if (input.files) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    this.uplodaImgPreview.preview = e.target.result;
                }
                this.uplodaImgPreview.image = input.files[0];
                this.createInfo.Img = input.files[0];
                reader.readAsDataURL(input.files[0]);
            }
        },
    },
    components: {
        tabcontent: {
            props: {
                data: ['tabType'],
            },
            data() {
                return {
                    selectedType: "首頁",
                    filteredAnnouncementes: [],
                    Announcementes: [],
                    keyword: "",
                    uplodaImgPreview: {
                        preview: null,
                        image: null,
                    },
                    createInfo: {                      
                        Id: 0,
                        Status: false,
                        Context: "請輸入內容",
                        Createtime: "",
                        Creator: "",
                        Img: null,
                        Type: "首頁",
                    },
                                 
                };
            },          
            computed: {
   
            },
            methods: {
            
                previewImage: function (e) {
                    var input = e.target;
                    if (input.files) {
                        var reader = new FileReader();
                        reader.onload = (e) => {
                            this.uplodaImgPreview.preview = e.target.result;
                        }
                        this.uplodaImgPreview.image = input.files[0];
                        this.createInfo.Img = input.files[0];
                        reader.readAsDataURL(input.files[0]);
                    }
                },
                CreateAA() {
                    this.createInfo.Createtime = Date.now();
                },
                GetSearchDetail() {
                    let _this = this;
                    axios.get("/BackStage/Announcement/GetFilteredAnnouncement", {
                    params: {
                        type: _this.selectedType,
                        keyword: _this.keyword
                    }
                }).then(response => {
                    this.filteredAnnouncementes = response.data;
                })
                },
                //拿資料
                GetAnouncementDetail() {
                    let _this = this;
                    axios.get("/BackStage/Announcement/GetAnnouncement").then(response => {
                        _this.Announcementes = response.data;
                    })                   
                },              
                //編輯按鈕
                EditAA(item) {
                    //把item的值丟入EditInfo
                    this.editImg = "data:image/png;base64," + item.img
                    this.uplodaImgPreview.preview = "data:image/png;base64," + item.img
                    this.EditProdcutItem = item
                    this.editId = item.productId
                    this.openModal()
                    this.storeOldEditData(item)
                },
                
                //刪除資料
                DeleteAnnouncement(id) {
                    let _this = this;
                    Swal.fire({
                        title: `確認刪除編號${id}？`,
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: '確定'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            axios.delete(`/BackStage/Announcement/DeleteAnnouncement?id=${id}`).then(res => {
                                Swal.fire(
                                    '資料已成功刪除!',
                                )
                                _this.GetAnouncementDetail()
                            });
                        }                            
                        })
                },
            },
            mounted() {
               this.GetSearchDetail()
               this.GetAnouncementDetail()
                this.$nextTick(() => {
                eventBus.$on('announcementesUpdated', newAnnouncementes => {
                    this.Announcementes = newAnnouncementes;
                });
                    eventBus.$on('tabChanged', newselectedType => {
                        this.selectedType = newselectedType;
                    });                   
                }); 
                //this.$bus.$on('tabChanged', this.handleTabChanged);
            },
        },
    }
})
    



