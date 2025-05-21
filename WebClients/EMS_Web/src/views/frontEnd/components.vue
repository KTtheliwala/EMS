<template>
  <div class="layout-main-content">
    <div class="subnavtab-wrapper">
      <div class="content-wrap">
        <TabView class="custom-tab">
          <TabPanel header="Components">
            <div class="grid guter-change">
                <div class="col-12 md:col-12">
                  <h3>Typography</h3>
                </div>
                <div class="col-12 md:col-6">
                  <div class="TypographyWrap">
                    <h1 class="mt-0 mb-0">Heading 1 - Bold, 24px</h1>
                    <h2>Heading 2 - Bold, 20px</h2>
                    <h3>Heading 3 - Bold, 18px</h3>
                    <h4>Heading 4 - normal, 16px</h4>
                    <h5>Heading 5 - normal, 14px</h5>
                    <h6>Heading 6 - normal, 12px</h6>
                  </div>
                </div>
                <div class="col-12 md:col-6">
                  <div class="BodyText">
                    <div>
                      <h4 class="mt-0">Body Text</h4>
                      <p>
                        Lorem Ipsum is simply dummy text of the printing and
                        typesetting industry. Lorem Ipsum has been the industry's
                        standard dummy text ever since the 1500s, when an unknown
                        printer took a galley of type and scrambled it to make a type
                        specimen book. It has survived not only five centuries, but
                        also the leap into electronic typesetting, remaining
                        essentially unchanged. It was popularised in the 1960s with
                        the release of
                      </p>
                    </div>
                    <div>
                      <h4 class="mt-0 mb-0">Text With Bullet</h4>
                      <ul class="list-disc">
                        <li>When an unknown printer</li>
                        <li>Publishing software like Aldus</li>
                        <li>Typesetting industry</li>
                      </ul>
                      <ul class="list-decimal">
                        <li>Ever since the 1500s</li>
                        <li>Publishing software like Aldus</li>
                        <li>Standard dummy text</li>
                      </ul>
                    </div>
                  </div>
                </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Buttons</h3>
              </div>
              <div class="col-12 md:col-12">
                <common-button
                  :btnlabel="'Primary Button'"
                  :btntype="'text'"
                  :btnclass="'p-button-primary mr-2'"
                ></common-button>
                <common-button
                  :btnlabel="'Secondary Button'"
                  :btntype="'text'"
                  :btnclass="'p-button-secondary mr-2'"
                ></common-button>
                <common-button
                  :btnlabel="'auxiliary Button'"
                  :btntype="'text'"
                  :btnclass="'p-button-aux mr-2'"
                ></common-button>
                <common-button
                  :btnlabel="'Danger Button'"
                  :btntype="'text'"
                  :btnclass="'p-button-danger mr-2'"
                ></common-button>
                <common-button
                  :btnlabel="'Text Button'"
                  :btntype="'text'"
                  :btnclass="'p-button-text mr-2'"
                ></common-button>
                <common-button
                  :btnlabel="'Withdraw'"
                  :icon="'pi pi-arrow-left'"
                  :btntype="'text'"
                  :btnclass="'p-button-secondary mr-2'"
                  @click="OpenMeterReadingPopup"
                ></common-button>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Input Field</h3>
              </div>
              <div class="col-12 md:col-3">
                <commonInput fieldlabel="Default"  inputclass="form-control" v-model="inputComponent"  />
              </div>
              <div class="col-12 md:col-3">
                <commonInput fieldlabel="Filled" v-model="FilledInput" />
              </div>
              <div class="col-12 md:col-3">
                <commonInput
                  fieldlabel="Disabled / Readonly"
                  v-model="DisabledInput"
                  Isdisabled
                />
              </div>
              <div class="col-12 md:col-3">
                <commonInput class="error" fieldlabel="Error" v-model="inputComponentError" />
                <p class="error-msg">Error Information</p>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Select Dropdown</h3>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Default</label>
                  <common-dropdown fieldlabel="Default" placeholder="Please Select"  v-model="SelectDropdown" :options="SelectDropdownOptions"/>
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Selected</label>
                  <common-dropdown fieldlabel="Selected" placeholder="Please Select"  v-model="SelectDropdownSelected" :options="SelectDropdownOptions"/>
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Disabled / Readonly</label>
                  <common-dropdown fieldlabel="Disabled / Readonly" placeholder="Please Select"  v-model="SelectDropdownDisabled" :options="SelectDropdownOptions" disabled/>
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Error</label>
                  <common-dropdown class="error" fieldlabel="Error" placeholder="Please Select"  v-model="SelectDropdownError" :options="SelectDropdownOptions"/>
                </div>
                <p class="error-msg">Error Information</p>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Group Selection Single</label>
                  <Dropdown v-model="SingleGroupDrop" :options="SingleGroupDropOption" optionLabel="label" optionGroupLabel="label" optionGroupChildren="items"  : >
                    <template #option="slotProps">
                        <div class="GroupDrop-items">
                            <div>{{slotProps.option.label}}</div>
                            <div class="openItem">{{slotProps.option.OpenItem}}</div>
                        </div>
                    </template>
                  </Dropdown>
                </div>
              </div>
              
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Multi Select Dropdown</h3>
              </div>
              <div class="col-12 md:col-3">
                <common-multi-select
                  fieldlabel="Default"
                  placeholder="Please Select"
                  
                  v-model="MultiSelectDropdown"
                  :options="MultiSelectDropdownOptions"
                />
              </div>
              <div class="col-12 md:col-3">
                <common-multi-select
                  fieldlabel="Selected"
                  placeholder="Please Select"
                  
                  v-model="MultiSelectDropdownSelected"
                  :options="MultiSelectDropdownOptions"
                />
              </div>
              <div class="col-12 md:col-3">
                <common-multi-select
                  fieldlabel="Disabled / Readonly"
                  placeholder="Please Select"
                  
                  v-model="MultiSelectDropdownDisabled"
                  :options="MultiSelectDropdownOptions"
                  disabled
                />
              </div>
              <div class="col-12 md:col-3">
                <common-multi-select
                  class="error"
                  fieldlabel="Error"
                  placeholder="Please Select"
                  
                  v-model="MultiSelectDropdownError"
                  :options="MultiSelectDropdownOptions"
                />
                <p class="error-msg">Error Information</p>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Group Selection Multiple</label>
                  <MultiSelect v-model="MultipleGroupDrop" :options="MultipleGroupDropOption" optionLabel="label" optionGroupLabel="label" optionGroupChildren="items":maxSelectedLabels="1" :showToggleAll="false">
                    <template #option="slotProps">
                        <div class="GroupDrop-items">
                            <div>{{slotProps.option.label}}</div>
                            <div class="openItem">{{slotProps.option.OpenItem}}</div>
                        </div>
                    </template>
                  </MultiSelect>
                </div>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Checkbox and Radio Buttons</h3>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-checkbox">
                  <Checkbox
                    inputId="Default"
                    name="Default"
                    value="Default"
                    v-model="checkbox"
                  />
                  <label class="m-0 ml-2" for="Default">Default</label>
                </div>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-checkbox">
                  <Checkbox
                    inputId="Checked"
                    name="Checked"
                    value="Checked"
                    v-model="checkbox"
                    checked
                  />
                  <label class="m-0 ml-2" for="Checked">Checked</label>
                </div>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-checkbox">
                  <Checkbox
                    inputId="Disabled"
                    name="Disabled"
                    value="Disabled"
                    v-model="checkbox"
                    :disabled="disabled"
                  />
                  <label class="m-0 ml-2" for="Disabled">Disabled</label>
                </div>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-radiobutton">
                  <RadioButton
                    inputId="DefaultRadio"
                    name="DefaultRadio"
                    value="DefaultRadio"
                    v-model="radiobutton"
                  />
                  <label class="m-0 ml-2" for="DefaultRadio">Default</label>
                </div>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-radiobutton">
                  <RadioButton
                    inputId="CheckedRadio"
                    name="CheckedRadio"
                    value="CheckedRadio"
                    v-model="radiobutton"
                  />
                  <label class="m-0 ml-2" for="CheckedRadio">Checked</label>
                </div>
              </div>
              <div class="col-12 md:col-2">
                <div class="field field-radiobutton">
                  <RadioButton
                    inputId="DisabledRadio"
                    name="DisabledRadio"
                    value="DisabledRadio"
                    v-model="radiobutton"
                    :disabled="disabled"
                  />
                  <label class="m-0 ml-2" for="DisabledRadio">Disabled</label>
                </div>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Date And Time Picker</h3>
              </div>
              <div class="col-12 md:col-4">
                <div class="field">
                  <label class="block w-full">Date</label>
                  <common-date
                    id="icon"
                    v-model="datePicker"
                    :showIcon="true"
                    class="w-full"
                    placeholder="Select Date"
                    dateFormat="dd M yy"
                  ></common-date>
                </div>
              </div>
              <div class="col-12 md:col-4">
                <div class="field">
                  <label class="block w-full">Time</label>
                  <common-date
                    id="icon"
                    v-model="timePicker"
                    :showIcon="true"
                    icon="pi pi-clock"
                    :timeOnly="true"
                    hourFormat="12"
                    class="w-full"
                    placeholder="Select Time"
                  ></common-date>
                </div>
              </div>
            </div>
            <hr />
            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Textarea and File Upload</h3>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Textarea</label>
                  <commonArea fieldlabel="Textarea" v-model="textarea"></commonArea>
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Disabled / Readonly</label>
                  <commonArea fieldlabel="Disabled / Readonly" v-model="textarea" Isdisabled></commonArea>
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div class="field">
                  <label class="block w-full">Choose File</label>
                  <FileUpload mode="basic" name="demo[]" url="./upload" />
                </div>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Modal and Toaster</h3>
              </div>
              <div class="col-12 md:col-4">
                <div class="field">
                  <Button
                    label="Show"
                    icon="pi pi-external-link"
                    @click="openModal"
                  />
                </div>
              </div>
              <div class="col-12 md:col-8">
                <div class="field">
                  <Button
                    label="Success"
                    class="p-button-success mr-2"
                    @click="showSuccess"
                  />
                  <Button
                    label="Info"
                    class="p-button-info mr-2"
                    @click="showInfo"
                  />
                  <Button
                    label="Warn"
                    class="p-button-warning mr-2"
                    @click="showWarn"
                  />
                  <Button
                    label="Error"
                    class="p-button-danger mr-2"
                    @click="showError"
                  />
                  <Button
                    label="Positions Bottom Right"
                    class="p-button-success mr-2"
                    @click="showBottomRight"
                  />
                  <Button @click="showTemplate" label="Template Confirm" />
                </div>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-12">
                <h3>Tabs & Accordion</h3>
              </div>
              <div class="col-12 md:col-6">
                <TabView class="custom-tab">
                  <TabPanel header="Tab One">
                    <div class="mt-2 mb-2">Lorem Ipsum is simply dummy text of the printing and typesetting
                    industry. Lorem Ipsum has been the industry's standard dummy
                    text ever since the 1500s, when an unknown printer took a galley
                    of type and scrambled it to make a type specimen book. It has
                    survived not only five centuries, but also the leap into
                    electronic typesetting, remaining essentially unchanged. It was
                    popularised in the 1960s with the release of Letraset sheets
                    containing Lorem Ipsum passages, and more recently with desktop
                    publishing software like Aldus PageMaker including versions of
                    Lorem Ipsum.</div>
                  </TabPanel>
                  <TabPanel header="Tab Two">
                    <div class="mt-2 mb-2">It is a long established fact that a reader will be distracted
                    by the readable content of a page when looking at its layout.
                    The point of using Lorem Ipsum is that it has a more-or-less
                    normal distribution of letters, as opposed to using 'Content
                    here, content here', making it look like readable English. Many
                    desktop publishing packages and web page editors now use Lorem
                    Ipsum as their default model text, and a search for 'lorem
                    ipsum' will uncover many web sites still in their infancy.
                    Various versions have evolved over the years, sometimes by
                    accident, sometimes on purpose (injected humour and the like).</div>
                  </TabPanel>
                </TabView>
              </div>
              <div class="col-12 md:col-6">
                <Accordion :activeIndex="0">
                  <AccordionTab header="Header I">
                    <p>
                      Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed
                      do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                      Ut enim ad minim veniam, quis nostrud exercitation ullamco
                      laboris nisi ut aliquip ex ea commodo consequat. Duis aute
                      irure dolor in reprehenderit in voluptate velit esse cillum
                      dolore eu fugiat nulla pariatur. Excepteur sint occaecat
                      cupidatat non proident, sunt in culpa qui officia deserunt
                      mollit anim id est laborum.
                    </p>
                  </AccordionTab>
                  <AccordionTab header="Header II">
                    <p>
                      Sed ut perspiciatis unde omnis iste natus error sit voluptatem
                      accusantium doloremque laudantium, totam rem aperiam, eaque
                      ipsa quae ab illo inventore veritatis et quasi architecto
                      beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem
                      quia voluptas sit aspernatur aut odit aut fugit, sed quia
                      consequuntur magni dolores eos qui ratione voluptatem sequi
                      nesciunt. Consectetur, adipisci velit, sed quia non numquam
                      eius modi.
                    </p>
                  </AccordionTab>
                  <AccordionTab header="Header III">
                    <p>
                      At vero eos et accusamus et iusto odio dignissimos ducimus qui
                      blanditiis praesentium voluptatum deleniti atque corrupti quos
                      dolores et quas molestias excepturi sint occaecati cupiditate
                      non provident, similique sunt in culpa qui officia deserunt
                      mollitia animi, id est laborum et dolorum fuga. Et harum
                      quidem rerum facilis est et expedita distinctio. Nam libero
                      tempore, cum soluta nobis est eligendi optio cumque nihil
                      impedit quo minus.
                    </p>
                  </AccordionTab>
                </Accordion>
              </div>
            </div>
            <hr />

            <div class="grid guter-change">
              <div class="col-12 md:col-4">
                <div class="form-layout mt-4">
                  <div class="form-header">
                    <h4 class="mt-0">Basic</h4>
                  </div>
                  <div class="form-body">
                    <Chart type="line" :data="basicData" :options="basicOptions" />
                  </div>
                </div>
              </div>
              <div class="col-12 md:col-4">
                <div class="form-layout mt-4">
                  <div class="form-header">
                    <h4 class="mt-0">Multi Axis</h4>
                  </div>
                  <div class="form-body">
                    <Chart
                      type="line"
                      :data="multiAxisData"
                      :options="multiAxisOptions"
                    />
                  </div>
                </div>
              </div>
              <div class="col-12 md:col-4">
                <div class="form-layout mt-4">
                  <div class="form-header">
                    <h4 class="mt-0">Line Styles</h4>
                  </div>
                  <div class="form-body">
                    <Chart
                      type="line"
                      :data="lineStylesData"
                      :options="basicOptions"
                    />
                  </div>
                </div>
              </div>
            </div>
          </TabPanel>
          <!-- <TabPanel header="Molecules">
            
          </TabPanel> -->
          <TabPanel header="Color Palette">
            <div class="grid guter-change mt-3 mb-3">
              <div class="col-12 md:col-3">
                <div
                  class="color-box"
                  style="background-color: var(--Primary-50); color: #000000"
                >
                  primary-50
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-100); color: #000000"
                >
                  primary-100
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-200); color: #000000"
                >
                  primary-200
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-300); color: #000000"
                >
                  primary-300
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-400); color: #000000"
                >
                  primary-400
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-500); color: #000000"
                >
                  primary-500
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-600); color: #ffffff"
                >
                  primary-600
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-700); color: #ffffff"
                >
                  primary-700
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-800); color: #ffffff"
                >
                  primary-800
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--Primary-900); color: #ffffff"
                >
                  primary-900
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div
                  class="color-box"
                  style="background-color: var(--secondary-50); color: #000000"
                >
                  secondary-50
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-100); color: #000000"
                >
                  secondary-100
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-200); color: #000000"
                >
                  secondary-200
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-300); color: #000000"
                >
                  secondary-300
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-400); color: #000000"
                >
                  secondary-400
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-500); color: #000000"
                >
                  secondary-500
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-600); color: #ffffff"
                >
                  secondary-600
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-700); color: #ffffff"
                >
                  secondary-700
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-800); color: #ffffff"
                >
                  secondary-800
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--secondary-900); color: #ffffff"
                >
                  secondary-900
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div
                  class="color-box"
                  style="background-color: var(--red-50); color: #000000"
                >
                  red-50
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-100); color: #000000"
                >
                  red-100
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-200); color: #000000"
                >
                  red-200
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-300); color: #000000"
                >
                  red-300
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-400); color: #000000"
                >
                  red-400
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-500); color: #000000"
                >
                  red-500
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-600); color: #ffffff"
                >
                  red-600
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-700); color: #ffffff"
                >
                  red-700
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-800); color: #ffffff"
                >
                  red-800
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--red-900); color: #ffffff"
                >
                  red-900
                </div>
              </div>
              <div class="col-12 md:col-3">
                <div
                  class="color-box"
                  style="background-color: var(--orange-50); color: #000000"
                >
                  orange-50
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-100); color: #000000"
                >
                  orange-100
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-200); color: #000000"
                >
                  orange-200
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-300); color: #000000"
                >
                  orange-300
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-400); color: #000000"
                >
                  orange-400
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-500); color: #000000"
                >
                  orange-500
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-600); color: #ffffff"
                >
                  orange-600
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-700); color: #ffffff"
                >
                  orange-700
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-800); color: #ffffff"
                >
                  orange-800
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--orange-900); color: #ffffff"
                >
                  orange-900
                </div>
              </div>
            </div>
            <div class="grid guter-change mt-3 mb-3">
              <div class="col-12 md:col-3">
                <div
                  class="color-box"
                  style="background-color: var(--green-50); color: #000000"
                >
                  green-50
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-100); color: #000000"
                >
                  green-100
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-200); color: #000000"
                >
                  green-200
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-300); color: #000000"
                >
                  green-300
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-400); color: #000000"
                >
                  green-400
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-500); color: #000000"
                >
                  green-500
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-600); color: #ffffff"
                >
                  green-600
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-700); color: #ffffff"
                >
                  green-700
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-800); color: #ffffff"
                >
                  green-800
                </div>
                <div
                  class="color-box"
                  style="background-color: var(--green-900); color: #ffffff"
                >
                  green-900
                </div>
              </div>
            </div>
          </TabPanel>
          <TabPanel header="Status">
            <div style="padding-top:20px; padding-bottom:20px;">
              <div class="status-section mb-2">
                <div class="status-item na">
                  <div class="flag">N.A.</div>
                  <div class="title">Sales</div>
                  <div class="status-label">N.A</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item draft">
                  <div class="flag">Draft</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SA - Sales Order</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SM - Review Sales Order</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SM - Review Sales Order</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SM - Review Sales Order</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SE - Sales Verification</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SE - Sales Verification</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SE - Sales Verification</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SA - Record Start Meter</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Sales</div>
                  <div class="status-label">SA - Record Start Meter</div>
                </div>
              </div>
              <hr>
              <div class="status-section mb-2">
                <div class="status-item na">
                  <div class="flag">N.A.</div>
                  <div class="title">Operations</div>
                  <div class="status-label">N.A</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Route Number</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Route Number</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Route Number</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Schedule</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Schedule</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Operations</div>
                  <div class="status-label">OM - Set Schedule</div>
                </div>
              </div>
              <hr>
              <div class="status-section mb-2">
                <div class="status-item na">
                  <div class="flag">N.A.</div>
                  <div class="title">Technical</div>
                  <div class="status-label">N.A</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TM - Set Schedule</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TM - Set Schedule</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TM - Set Schedule</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Assign Tech</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Assign Tech</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TI - Setup Machine</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TI - Setup Machine</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Review Machine</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Review Machine</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Set Machine In Use</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Technical</div>
                  <div class="status-label">TA - Set Machine In Use</div>
                </div>
              </div>
              <hr>
              <div class="status-section mb-2">
                <div class="status-item na">
                  <div class="flag">N.A.</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">N.A</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Set Schedule</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Set Schedule</div>
                </div>
                <div class="status-item rejected">
                  <div class="flag">Rejected</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Set Schedule</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Delivery of Machine</div>
                </div>
                <div class="status-item completed">
                  <div class="flag">Completed</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Delivery of Machine</div>
                </div>
              </div>
              <div class="status-section mb-2">
                <div class="status-item" style="border:0;">
                </div>
                <div class="status-item pending">
                  <div class="flag">Pending</div>
                  <div class="title">Logistics</div>
                  <div class="status-label">LM - Machine Delivered</div>
                </div>
              </div>
            </div>
            
          </TabPanel>
        </TabView>

        <Sidebar v-model:visible="ComplaintsImportLogs" position="right">
          <div class="filter-category">
            <div class="field">
              <span class="p-float-label">
                <InputText id="ComplaintID" type="text" v-model="ComplaintID" />
                <label for="ComplaintID">Complaint ID</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="Machine No" type="text" v-model="MachineNo" />
                <label for="Machine No">Machine No</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="Company" type="text" v-model="Company" />
                <label for="Company">Company</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="Branchname" type="text" v-model="Branchname" />
                <label for="Branchname">Branch Name</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="Location" type="text" v-model="Location" />
                <label for="Location">Location</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="PostalCode" type="text" v-model="PostalCode" />
                <label for="PostalCode">Postal Code</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="Route" type="text" v-model="Route" />
                <label for="Route">Route</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="CS-Name" type="text" v-model="CSName" />
                <label for="CS-Name">CS Name</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText id="CustomerName" type="text" v-model="CustomerName" />
                <label for="CustomerName">Customer Name</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <InputText
                  id="CustomerContact"
                  type="text"
                  v-model="CustomerContact"
                />
                <label for="CustomerContact">Customer Contact</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <Dropdown
                  id="Source"
                  v-model="Source"
                  :options="SourceOptions"
                  optionLabel="name"
                />
                <label for="Source">Source</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <common-date
                  id="CreatedDate"
                  v-model="CreatedDate"
                  dateFormat="dd M yy"
                  selectionMode="range"
                ></common-date>
                <label for="CreatedDate">Created Date (Range)</label>
              </span>
            </div>
            <div class="field">
              <span class="p-float-label">
                <Dropdown
                  id="Status"
                  v-model="Status"
                  :options="StatusOptions"
                  optionLabel="name"
                />
                <label for="Status">Status</label>
              </span>
            </div>
          </div>
          <div class="filterFTR">
            <Button
              label="Close Filters"
              class="p-button-text ClearBTN"
              @click="ComplaintsImportLogs = !ComplaintsImportLogs"
            />
            <Button
              label="Apply Filter"
              class="mr-0"
              autofocus
              @click="ComplaintsImportLogs = !ComplaintsImportLogs"
            />
            <Button
              label="Clear"
              class="p-button-aux mr-0"
              autofocus
              @click="ClearFilter"
            />
          </div>
        </Sidebar>

        <Dialog
          header="Modal Header"
          v-model:visible="displayModal"
          :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
          :style="{ width: '50vw' }"
          :modal="true"
        >
          <p class="m-0">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod
            tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
            veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
            commodo consequat. Duis aute irure dolor in reprehenderit in voluptate
            velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint
            occaecat cupidatat non proident, sunt in culpa qui officia deserunt
            mollit anim id est laborum.
          </p>
          <div class="p-dialog-footer">
            <div></div>
            <div>
              <Button label="Cancel" class="p-button-aux" @click="closeModal" />
              <Button label="Save" class="p-button-primary mr-0" @click="closeModal" autofocus />
            </div>
          </div>
        </Dialog>

        <Toast />
        <Toast position="bottom-right" group="br" />
        <Toast position="bottom-center" group="bc">
          <template #message="slotProps">
            <div class="flex flex-column w-100">
              <div class="text-center">
                <i class="pi pi-exclamation-triangle" style="font-size: 3rem"></i>
                <h4>{{ slotProps.message.summary }}</h4>
                <p>{{ slotProps.message.detail }}</p>
              </div>
              <div class="grid p-fluid">
                <div class="col-6">
                  <Button
                    class="p-button-success"
                    label="Yes"
                    @click="onConfirm"
                  ></Button>
                </div>
                <div class="col-6">
                  <Button
                    class="p-button-aux"
                    label="No"
                    @click="onReject"
                  ></Button>
                </div>
              </div>
            </div>
          </template>
        </Toast>
    </div>
    </div>
  </div>


  <!-- Meter Reading Start -->
  <Dialog class="large-modal" :header="'Withdraw'" v-model:visible="AddMeterReadingPopup" :modal="true">
    <div class="grid guter-change">
      <div class="col-12 md:col-6">
        <div class="field">
          <label class="block w-full">Withdrawal Date</label>
          <common-date v-model="WithdrawalDate" dateFormat="dd M yy" :selectOtherMonths="true" :showButtonBar="true"  selectionMode="range" :showIcon="true" class="w-full"></common-date>
        </div>
      </div>
      <div class="col-12 md:col-6">
        <div class="field">
          <label class="block w-full">SE Remarks</label>
          <commonArea v-model="SERemarkC"></commonArea>
        </div>
      </div>
    </div>  
    <div class="p-dialog-footer">
      <div></div>
      <div>
        <Button label="Cancel" class="p-button-aux" icon="pi pi-times-circle" @click="CloseMeterReadingPopup"/>
        <Button label="Add" icon="pi pi-plus-circle" @click="CloseMeterReadingPopup" autofocus />
      </div>
    </div>
  </Dialog>
    <!-- Meter Reading End -->

</template>
<script setup lang="ts">
import { ref, reactive } from "vue";
import { FilterMatchMode } from "primevue/api";
import { useToast } from "primevue/usetoast";

// Filter Sidebar
const ComplaintsImportLogs = ref(false);

// Input Field
const FilledInput = ref("Filled Value");
const ReadonlyInput = ref("Read Only Value");
const DisabledInput = ref("Disabled Value");

// Select Dropdown
const SelectDropdown = ref();
const SelectDropdownSelected = ref(1);
const SelectDropdownReadOnly = ref();
const SelectDropdownDisabled = ref();
const SelectDropdownError = ref();
const SelectDropdownOptions = ref([
  { Text: "Option A", Value: "1" },
  { Text: "Option B", Value: "2" },
  { Text: "Option C", Value: "3" },
]);

const SingleGroupDrop = ref();
const SingleGroupDropOption = ref([
  {
      label: 'Default Attendants', code: 'DA',
      items: [
          {label: 'Default User 1', value: '1', OpenItem: '3 Open'},
          {label: 'Default User 2', value: '2', OpenItem: '1 Open'},
          {label: 'Default User 3', value: '3', OpenItem: '2 Open'},
      ]
  },
  {
      label: 'Other Attendants', code: 'OA',
      items: [
          {label: 'Other User 1', value: '1', OpenItem: '5 Open'},
          {label: 'Other User 2', value: '2', OpenItem: '10 Open'},
          {label: 'Other User 3', value: '3', OpenItem: '1 Open'},
      ]
  },
]);

const MultipleGroupDrop = ref();
const MultipleGroupDropOption = ref([
  {
      label: 'Default Attendants', code: 'DA',
      items: [
          {label: 'Default User 1', value: '1', OpenItem: '3 Open'},
          {label: 'Default User 2', value: '2', OpenItem: '1 Open'},
          {label: 'Default User 3', value: '3', OpenItem: '2 Open'},
      ]
  },
  {
      label: 'Other Attendants', code: 'OA',
      items: [
          {label: 'Other User 1', value: '1', OpenItem: '5 Open'},
          {label: 'Other User 2', value: '2', OpenItem: '10 Open'},
          {label: 'Other User 3', value: '3', OpenItem: '1 Open'},
      ]
  },
]);


// Multi Select Dropdown
const MultiSelectDropdown = ref();
const MultiSelectDropdownSelected = ref([1, 2]);
const MultiSelectDropdownReadOnly = ref();
const MultiSelectDropdownDisabled = ref();
const MultiSelectDropdownError = ref();
const MultiSelectDropdownOptions = ref([
  { Text: "Option A", Value: "1" },
  { Text: "Option B", Value: "2" },
  { Text: "Option C", Value: "3" },
]);

// Checkbox and Radio Button
const checkbox = ref();
const radiobutton = ref();
const disabled = ref(true);

// Date Time Picker
const datePicker = ref();
const timePicker = ref();

// Textarea
const textarea = ref([]);

// Data Table
const ImportLogsselectedUsers = ref([]);
const ImportLogsDatas = ref([
  {
    id: "1",
    No: "25",
    Mon: "21034",
    MonDesc: "AMS SENSORS SINGAPORE PTE LTD",
    Tue: "20905",
    TueDesc: "BARRY CALLEBAUT CHOCOLATE",
    Wed: "40456",
    WedDesc: "LLUMNA SINGAPORE PTE LTD",
    Thur: "21034",
    ThurDesc: "AMS SENSORS SINAPORE PTE LTD",
    Fri: "40475",
    FriDesc: "LLUMNA SINGAPORE PTE LTD",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "2",
    No: "26",
    Mon: "21048",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "20918",
    TueDesc: "CCL DESIGN SINGAPORE PTE LTD",
    Wed: "40475",
    WedDesc: "LLUMNA SINGAPORE PTE LTD",
    Thur: "21084",
    ThurDesc: "ZOOM ENGINEERING PTE LTD",
    Fri: "40479",
    FriDesc: "LLUMNA SINGAPORE PTE LTD",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "3",
    No: "27",
    Mon: "21048",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "20951",
    TueDesc: "UNIVAC PRECISION ENGINEERING",
    Wed: "40479",
    WedDesc: "LLUMNA SINGAPORE PTE LTD",
    Thur: "",
    ThurDesc: "7",
    Fri: "40486",
    FriDesc: "OBAYASHI SINGAPORE PTE LTD",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "3",
    No: "28",
    Mon: "21055",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "21078",
    TueDesc: "UNIVAC PRECISION ENGINEERING",
    Wed: "40486",
    WedDesc: "OBAYASHI SINGAPORE PTE LTD",
    Thur: "40461",
    ThurDesc: "BARRY CALLEBAUT CHOCOLATE",
    Fri: "",
    FriDesc: "6",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "4",
    No: "29",
    Mon: "",
    MonDesc: "12",
    Tue: "",
    TueDesc: "11",
    Wed: "40600",
    WedDesc: "ASIA FOOD CAFETERIA",
    Thur: "40614",
    ThurDesc: "AMS SENSORS SINGAPORE PTE LTD",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "5",
    No: "30",
    Mon: "40321",
    MonDesc: "ICHOR SYSTEMS SINGAPORE PTE LTD",
    Tue: "40680",
    TueDesc: "THE KENG HONG",
    Wed: "",
    WedDesc: "2",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "6",
    No: "31",
    Mon: "40456",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "40283",
    TueDesc: "WOODLANDS ZONE '2' RC",
    Wed: "40733",
    WedDesc: "The Management Corporation Str",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "7",
    No: "32",
    Mon: "40457",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "40770",
    TueDesc: "PRO*3 INSTITUTIONAL CATERING P",
    Wed: "",
    WedDesc: "8",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "8",
    No: "33",
    Mon: "40475",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "40629",
    TueDesc: "RISEWOOD MCST 2839",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "9",
    No: "34",
    Mon: "40479",
    MonDesc: "LLUMNA SINGAPORE PTE LTD",
    Tue: "",
    TueDesc: "4",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "10",
    No: "35",
    Mon: "40486",
    MonDesc: "OBAYASHI SINGAPORE PTE LTD",
    Tue: "",
    TueDesc: "",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "11",
    No: "36",
    Mon: "40614",
    MonDesc: "AMS SENSORS SINGAPORE PTE LTD",
    Tue: "",
    TueDesc: "",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "12",
    No: "37",
    Mon: "40621",
    MonDesc: "AMS SENSORS SINGAPORE PTE LTD",
    Tue: "",
    TueDesc: "",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "13",
    No: "38",
    Mon: "40714",
    MonDesc: "",
    Tue: "",
    TueDesc: "",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "14",
    No: "39",
    Mon: "",
    MonDesc: "9",
    Tue: "",
    TueDesc: "",
    Wed: "",
    WedDesc: "",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
  {
    id: "15",
    No: "36",
    Mon: "",
    MonDesc: "31",
    Tue: "",
    TueDesc: "29",
    Wed: "",
    WedDesc: "27",
    Thur: "",
    ThurDesc: "",
    Fri: "",
    FriDesc: "",
    Sat: "",
    SatDesc: "",
    Sun: "",
    SunDesc: "",
  },
]);

// List/Grid View Data Table
const responsiveLayoutValue = ref("scroll");
const viewItem = ref(true);
const listViewOn = () => {
  viewItem.value = false;
  responsiveLayoutValue.value = "stack";
};
const gridViewOn = () => {
  viewItem.value = true;
  responsiveLayoutValue.value = "scroll";
};

// Auto Complet
const selectedItem = ref();
const filteredItem = ref();
const Items = ref([
  { name: "ABC PTE LTD", code: "20001" },
  { name: "DEF PTE LTD", code: "20002" },
  { name: "GHI PTE LTD", code: "20003" },
  { name: "JKL PTE LTD", code: "20004" },
  { name: "MNO PTE LTD", code: "20005" },
]);
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const searchItem = (event) => {
  setTimeout(() => {
    if (!event.query.trim().length) {
      filteredItem.value = [...Items.value];
    } else {
      filteredItem.value = Items.value.filter((country) => {
        return country.code.toLowerCase().startsWith(event.query.toLowerCase());
      });
    }
  }, 250);
};

// Chart
const basicData = ref({
  labels: ["January", "February", "March", "April", "May", "June", "July"],
  datasets: [
    {
      label: "First Dataset",
      data: [65, 59, 80, 81, 56, 55, 40],
      fill: false,
      borderColor: "#42A5F5",
      tension: 0.4,
    },
    {
      label: "Second Dataset",
      data: [28, 48, 40, 19, 86, 27, 90],
      fill: false,
      borderColor: "#FFA726",
      tension: 0.4,
    },
  ],
});

const multiAxisData = ref({
  labels: ["January", "February", "March", "April", "May", "June", "July"],
  datasets: [
    {
      label: "Dataset 1",
      fill: false,
      borderColor: "#42A5F5",
      yAxisID: "y",
      tension: 0.4,
      data: [65, 59, 80, 81, 56, 55, 10],
    },
    {
      label: "Dataset 2",
      fill: false,
      borderColor: "#00bb7e",
      yAxisID: "y1",
      tension: 0.4,
      data: [28, 48, 40, 19, 86, 27, 90],
    },
  ],
});

const lineStylesData = ref({
  labels: ["January", "February", "March", "April", "May", "June", "July"],
  datasets: [
    {
      label: "First Dataset",
      data: [65, 59, 80, 81, 56, 55, 40],
      fill: false,
      tension: 0.4,
      borderColor: "#42A5F5",
    },
    {
      label: "Second Dataset",
      data: [28, 48, 40, 19, 86, 27, 90],
      fill: false,
      borderDash: [5, 5],
      tension: 0.4,
      borderColor: "#66BB6A",
    },
    {
      label: "Third Dataset",
      data: [12, 51, 62, 33, 21, 62, 45],
      fill: true,
      borderColor: "#FFA726",
      tension: 0.4,
      backgroundColor: "rgba(255,167,38,0.2)",
    },
  ],
});

// Model
const displayModal = ref(false);
const openModal = () => {
  displayModal.value = true;
};
const closeModal = () => {
  displayModal.value = false;
};

// Toast
const toast = useToast();
const showSuccess = () => {
  toast.add({
    severity: "success",
    summary: "Success Message",
    detail: "Message Content",
    life: 3000,
  });
};
const showInfo = () => {
  toast.add({
    severity: "info",
    summary: "Info Message",
    detail: "Message Content",
    life: 3000,
  });
};
const showWarn = () => {
  toast.add({
    severity: "warn",
    summary: "Warn Message",
    detail: "Message Content",
    life: 3000,
  });
};
const showError = () => {
  toast.add({
    severity: "error",
    summary: "Error Message",
    detail: "Message Content",
    life: 3000,
  });
};
const showBottomRight = () => {
  toast.add({
    severity: "success",
    summary: "Success Message",
    detail: "Message Content",
    group: "br",
    life: 3000,
  });
};
const showTemplate = () => {
  toast.add({
    severity: "warn",
    summary: "Are you sure?",
    detail: "Proceed to confirm",
    group: "bc",
  });
};
const onConfirm = () => {
  toast.removeGroup("bc");
};
const onReject = () => {
  toast.removeGroup("bc");
};

const inputComponent = ref();
const inputComponentError = ref();





const AddMeterReadingPopup = ref(false);
const OpenMeterReadingPopup = () => {
    AddMeterReadingPopup.value = true;
};
const CloseMeterReadingPopup = () => {
    AddMeterReadingPopup.value = false;
};	

</script>
